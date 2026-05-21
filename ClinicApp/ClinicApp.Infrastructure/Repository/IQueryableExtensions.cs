using ClinicApp.Domain.Attribute;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Enum;
using ClinicApp.Domain.Request;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Repository;

internal static class IQueryableExtensions
{
    public static IQueryable<TEntity> FilterWithRequest<TEntity, TRequest>(this IQueryable<TEntity> query, TRequest request)
        where TEntity : class, IEntity
        where TRequest : PagedRequest
    {
        var entityProps = typeof(TEntity).GetProperties();
        var requestProps = typeof(TRequest).GetProperties()
            .Select(p =>
            {
                var attribute = p.GetCustomAttribute<RequestFieldAttribute>();

                (PropertyInfo Prop, RequestFieldAttribute? Attribute) t = (p, attribute);

                return t;
            })
            .Where(p => p.Prop.GetValue(request) is not null && (p.Attribute is null || !p.Attribute.Skip))
            .ToImmutableList();

        var entityParamExpression = Expression.Parameter(typeof(TEntity), "entity");

        foreach (var requestProp in requestProps)
        {
            var attribute = requestProp.Attribute;

            var propertyName = requestProp.Prop.Name;
            if(attribute is not null && !string.IsNullOrEmpty(attribute.Property))
            {
                propertyName = attribute.Property;
            }

            var entityPropExpression = Expression.Property(entityParamExpression, propertyName);
            var requestPropExpression = Expression.Constant(requestProp.Prop.GetValue(request)!);

            var comparison = attribute is null ? ComparisonType.Equal : attribute.ComparisonType;

            Expression comparisonExpression = comparison switch
            {
                ComparisonType.Equal => Expression.Equal(entityPropExpression, requestPropExpression),
                ComparisonType.LesserThan => Expression.LessThan(entityPropExpression, requestPropExpression),
                ComparisonType.GreaterThan => Expression.GreaterThan(entityPropExpression, requestPropExpression),
                ComparisonType.Contains => Expression.Call(requestPropExpression, requestProp.Prop.PropertyType.GetMethod("Contains")!,
                    entityPropExpression),
                ComparisonType.StartsWith => Expression.Call(entityPropExpression, typeof(string).GetMethod("StartsWith", [typeof(string)])!,
                    requestPropExpression),
                _ => throw new NotImplementedException()
            };

            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(comparisonExpression, entityParamExpression);

            query = query.Where(lambdaExpression);
        }

        return query;
    }

    public static IQueryable<TEntity> AddPagination<TEntity>(this IQueryable<TEntity> query, PagedRequest pagedRequest)
    {
        var index = pagedRequest.Index ?? 0;
        var pageSize = pagedRequest.PageSize ?? 10;

        return query.Skip(index * pageSize).Take(pageSize);
    }

    public static IQueryable<TEntity> WithIncludes<TEntity>(this IQueryable<TEntity> query, List<string> includes)
        where TEntity : class, IEntity
    {
        foreach(var include in includes)
        {
            query = query.Include(include);
        }

        return query;
    }
}

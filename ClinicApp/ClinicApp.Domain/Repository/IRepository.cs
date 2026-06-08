using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
using System.Linq.Expressions;

namespace ClinicApp.Domain.Repository;

public interface IRepository<TEntity> where TEntity : IEntity
{
    Task AddAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TModel?> GetByIdAsync<TModel>(Guid id, Expression<Func<TEntity, TModel>> selector);
    Task<TModel?> GetByIdAsync<TModel>(Guid id, Expression<Func<TEntity, TModel>> selector, List<string> includes);
}

public interface IRepository<TEntity, TGetRequest> : IRepository<TEntity> 
    where TEntity : IEntity
    where TGetRequest : PagedRequest
{
    Task<List<TEntity>> GetAsync(TGetRequest request);
    Task<List<TModel>> GetAsync<TModel>(TGetRequest request, Expression<Func<TEntity, TModel>> selector);
    Task<List<TModel>> GetAsync<TModel>(TGetRequest request, Expression<Func<TEntity, TModel>> selector, List<string> includes);
}

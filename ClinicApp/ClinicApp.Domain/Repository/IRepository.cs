using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
using System.Linq.Expressions;

namespace ClinicApp.Domain.Repository;

public interface IReadRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TModel?> GetByIdAsync<TModel>(Guid id, Expression<Func<TEntity, TModel>> selector);
    Task<TModel?> GetByIdAsync<TModel>(Guid id, Expression<Func<TEntity, TModel>> selector, List<string> includes);
    Task<int> GetCountAsync();
}

public interface IReadRepository<TEntity, TGetRequest> : IReadRepository<TEntity> where TEntity : class, IEntity
    where TGetRequest : PagedRequest
{
    Task<List<TEntity>> GetAsync(TGetRequest request);
    Task<List<TModel>> GetAsync<TModel>(TGetRequest request, Expression<Func<TEntity, TModel>> selector);
    Task<List<TModel>> GetAsync<TModel>(TGetRequest request, Expression<Func<TEntity, TModel>> selector, List<string> includes);
    Task<int> GetCountAsync(TGetRequest request);
}

public interface IRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IEntity
{
    Task AddAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
}

public interface IRepository<TEntity, TGetRequest> : IRepository<TEntity>, IReadRepository<TEntity, TGetRequest>
    where TEntity : class, IEntity
    where TGetRequest : PagedRequest
{

}

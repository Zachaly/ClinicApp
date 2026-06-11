using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Domain.Repository;

public interface IReadRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<TEntity?> GetByIdAsync(Guid id, List<string> includes);
    Task<int> GetCountAsync();
}

public interface IReadRepository<TEntity, TGetRequest> : IReadRepository<TEntity> where TEntity : class, IEntity
    where TGetRequest : PagedRequest
{
    Task<List<TEntity>> GetAsync(TGetRequest request);
    Task<List<TEntity>> GetAsync(TGetRequest request, List<string> includes);
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

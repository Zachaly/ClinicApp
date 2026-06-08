using ClinicApp.Database;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;

namespace ClinicApp.Infrastructure.Repository.Base;

public class RepositoryBase<TEntity, TGetRequest> : ReadRepositoryBase<TEntity, TGetRequest>, IRepository<TEntity, TGetRequest>
    where TEntity : class, IEntity
    where TGetRequest : PagedRequest
{
    public RepositoryBase(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbContext.Remove(entity);

        await _dbContext.SaveChangesAsync();
    }

    public Task UpdateAsync(TEntity entity)
    {
        _dbContext.Update(entity);

        return _dbContext.SaveChangesAsync();
    }
}

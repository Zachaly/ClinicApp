using ClinicApp.Database;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.Infrastructure.Repository.Base;

public abstract class ReadRepositoryBase<TEntity> : IReadRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly ApplicationDbContext _dbContext;

    protected ReadRepositoryBase(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual Task<TEntity?> GetByIdAsync(Guid id)
        => _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);

    public virtual Task<TEntity?> GetByIdAsync(Guid id, List<string> includes)
        => _dbContext.Set<TEntity>()
            .WithIncludes(includes)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();

    public Task<int> GetCountAsync()
        => _dbContext.Set<TEntity>().CountAsync();
}

public abstract class ReadRepositoryBase<TEntity, TGetRequest> : ReadRepositoryBase<TEntity>, IReadRepository<TEntity, TGetRequest>
    where TEntity : class, IEntity
    where TGetRequest : PagedRequest
{
    protected ReadRepositoryBase(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public virtual Task<List<TEntity>> GetAsync(TGetRequest request)
        => _dbContext.Set<TEntity>()
            .FilterWithRequest(request)
            .AddPagination(request)
            .ToListAsync();

    public virtual Task<List<TEntity>> GetAsync(TGetRequest request, List<string> includes)
        => _dbContext.Set<TEntity>()
            .WithIncludes(includes)
            .FilterWithRequest(request)
            .AddPagination(request)
            .ToListAsync();

    public virtual Task<int> GetCountAsync(TGetRequest request)
        => _dbContext.Set<TEntity>()
            .FilterWithRequest(request)
            .CountAsync();
}

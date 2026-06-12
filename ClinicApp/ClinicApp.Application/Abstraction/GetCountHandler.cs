using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Abstraction;

public abstract class GetCountHandler<TEntity, TGetRequest, TGetCountRequest>
    where TEntity : class, IEntity
    where TGetRequest : PagedRequest
    where TGetCountRequest : TGetRequest
{
    private readonly IReadRepository<TEntity, TGetRequest> _repository;

    protected GetCountHandler(IReadRepository<TEntity, TGetRequest> repository)
    {
        _repository = repository;
    }

    public virtual Task<int> Handle(TGetCountRequest request)
        => _repository.GetCountAsync(request);
}

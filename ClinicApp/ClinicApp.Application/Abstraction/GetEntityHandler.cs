using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Abstraction;

public abstract class GetEntityHandler<TEntity, TGetRequest, TModel> 
    where TEntity : class, IEntity
    where TGetRequest : PagedRequest
{
    private readonly IReadRepository<TEntity, TGetRequest> _repository;
    private readonly IModelMapper<TEntity, TModel> _mapper;
    private readonly List<string> _includes;

    protected GetEntityHandler(IReadRepository<TEntity, TGetRequest> repository, IModelMapper<TEntity, TModel> mapper, List<string> includes)
    {
        _repository = repository;
        _mapper = mapper;
        _includes = includes;
    }

    protected GetEntityHandler(IReadRepository<TEntity, TGetRequest> repository, IModelMapper<TEntity, TModel> mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _includes = [];
    }

    public virtual async Task<List<TModel>> Handle(TGetRequest request)
    {
        var entitites = await _repository.GetAsync(request, _includes);

        return entitites.Select(_mapper.MapEntityToModel).ToList();
    }
}

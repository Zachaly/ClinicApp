using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Abstraction;

public record GetEntityByIdRequest(Guid Id);

public abstract class GetEntityByIdHandler<TEntity, TModel, TRequest> 
    where TEntity : class, IEntity
    where TRequest : GetEntityByIdRequest
    where TModel : class
{
    private readonly IReadRepository<TEntity> _repository;
    private readonly IModelMapper<TEntity, TModel> _mapper;
    private readonly List<string> _includes;

    protected GetEntityByIdHandler(IReadRepository<TEntity> repository, IModelMapper<TEntity, TModel> mapper, List<string> includes)
    {
        _repository = repository;
        _mapper = mapper;
        _includes = includes;
    }

    protected GetEntityByIdHandler(IReadRepository<TEntity> repository, IModelMapper<TEntity, TModel> mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _includes = [];
    }

    public virtual async Task<TModel?> Handle(TRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id, _includes);

        return entity is null ? null : _mapper.MapEntityToModel(entity);
    }
}

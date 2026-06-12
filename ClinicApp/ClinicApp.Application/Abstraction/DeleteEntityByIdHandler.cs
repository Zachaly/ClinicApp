using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;

namespace ClinicApp.Application.Abstraction;

public record DeleteEntityByIdRequest(Guid Id);

public abstract class DeleteEntityByIdHandler<TEntity, TRequest>
    where TEntity : class, IEntity
    where TRequest : DeleteEntityByIdRequest
{
    private readonly IWriteRepository<TEntity> _repository;

    protected DeleteEntityByIdHandler(IWriteRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public virtual async Task<ResponseModel> Handle(TRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if(entity is null)
        {
            return new ResponseModel();
        }

        await _repository.DeleteAsync(entity);

        return new ResponseModel();
    }
}

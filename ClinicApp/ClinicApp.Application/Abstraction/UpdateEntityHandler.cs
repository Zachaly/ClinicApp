using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Abstraction;

public abstract class UpdateEntityHandler<TEntity, TUpdateRequest>
    where TEntity : class, IEntity
    where TUpdateRequest : IUpdateRequest
{
    private readonly IWriteRepository<TEntity> _repository;
    private readonly IValidator<TUpdateRequest> _validator;

    protected UpdateEntityHandler(IWriteRepository<TEntity> repository, IValidator<TUpdateRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public virtual async Task<ValidationResponseModel> Handle(TUpdateRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if(entity is null)
        {
            return new ValidationResponseModel("Entity not found");
        }

        var validation = _validator.Validate(request);

        if(!validation.IsValid)
        {
            return new ValidationResponseModel(validation.ToDictionary());
        }

        UpdateEntity(entity, request);

        await _repository.UpdateAsync(entity);

        return new ValidationResponseModel();
    }

    protected abstract void UpdateEntity(TEntity entity, TUpdateRequest request);
}

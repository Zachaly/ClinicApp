using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Abstraction;

public abstract class AddEntityHandler<TEntity, TAddRequest> where TEntity : class, IEntity 
{
    protected readonly IWriteRepository<TEntity> _repository;
    protected readonly IValidator<TAddRequest> _validator;
    protected readonly IRequestMapper<TEntity, TAddRequest> _mapper;

    protected AddEntityHandler(IWriteRepository<TEntity> repository,
        IValidator<TAddRequest> validator,
        IRequestMapper<TEntity, TAddRequest> mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
    }

    public virtual async Task<ValidationResponseModel> Handle(TAddRequest request)
    {
        var validation = _validator.Validate(request);

        if(!validation.IsValid)
        {
            return new ValidationResponseModel(validation.ToDictionary());
        }

        var entity = _mapper.MapRequestToEntity(request);

        await _repository.AddAsync(entity);

        return new ValidationResponseModel();
    }
}

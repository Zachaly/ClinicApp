using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class UpdatePatientHandler
{
    private readonly IPatientRepository _repository;
    private readonly IValidator<UpdatePatientRequest> _validator;

    public UpdatePatientHandler(IPatientRepository repository, IValidator<UpdatePatientRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<ValidationResponseModel> Handle(UpdatePatientRequest request)
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


        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Address = request.Address;
        entity.PostalCode = request.PostalCode;
        entity.City = request.City;

        await _repository.UpdateAsync(entity);

        return new ValidationResponseModel();
    }
}

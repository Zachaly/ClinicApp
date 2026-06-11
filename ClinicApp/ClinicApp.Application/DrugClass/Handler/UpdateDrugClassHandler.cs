using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class UpdateDrugClassHandler
{
    private readonly IDrugClassRepository _repository;
    private readonly IValidator<UpdateDrugClassRequest> _validator;

    public UpdateDrugClassHandler(IDrugClassRepository repository, IValidator<UpdateDrugClassRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<ValidationResponseModel> Handle(UpdateDrugClassRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if(entity is null)
        {
            return new ValidationResponseModel("Entity not found");
        }

        var classes = await _repository.GetAsync(new GetDrugClassRequest { Name = request.Name });

        if(classes.Any())
        {
            return new ValidationResponseModel("Name taken");
        }

        var validation = _validator.Validate(request);

        if(!validation.IsValid)
        {
            return new ValidationResponseModel(validation.ToDictionary());
        }

        entity.Name = request.Name;

        await _repository.UpdateAsync(entity);

        return new ValidationResponseModel();
    } 
}

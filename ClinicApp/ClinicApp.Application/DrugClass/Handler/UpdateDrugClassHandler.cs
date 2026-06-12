using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class UpdateDrugClassHandler : UpdateEntityHandler<DrugClass, UpdateDrugClassRequest>
{
    private readonly IDrugClassRepository _repository;

    public UpdateDrugClassHandler(IDrugClassRepository repository, IValidator<UpdateDrugClassRequest> validator) : base(repository, validator)
    {
        _repository = repository;
    }

    public override async Task<ValidationResponseModel> Handle(UpdateDrugClassRequest request)
    {
        var classes = await _repository.GetAsync(new GetDrugClassRequest { Name = request.Name });

        if(classes.Any())
        {
            return new ValidationResponseModel("Name taken");
        }

        return await base.Handle(request);
    }

    protected override void UpdateEntity(DrugClass entity, UpdateDrugClassRequest request)
    {
        entity.Name = request.Name;
    }
}

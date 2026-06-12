using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class UpdateDrugHandler : UpdateEntityHandler<Drug, UpdateDrugRequest>
{
    private readonly IDrugRepository _drugRepository;
    private readonly IDrugClassRepository _drugClassRepository;

    public UpdateDrugHandler(IDrugRepository drugRepository, IDrugClassRepository drugClassRepository, IValidator<UpdateDrugRequest> validator)
        : base(drugRepository, validator)
    {
        _drugRepository = drugRepository;
        _drugClassRepository = drugClassRepository;
    }

    public async Task<ValidationResponseModel> Handle(UpdateDrugRequest request)
    {
        var entity = await _drugRepository.GetByIdAsync(request.Id);

        if(entity is null)
        {
            return new ValidationResponseModel("Entity not found");
        }

        var drugClass = await _drugClassRepository.GetByIdAsync(request.ClassId);

        if(drugClass is null)
        {
            return new ValidationResponseModel("Drug class not found");
        }

        var drugs = await _drugRepository.GetAsync(new GetDrugRequest { BrandNameExact = request.BrandName });

        if(drugs.Any(d => d.Id != request.Id))
        {
            return new ValidationResponseModel("Name already taken");
        }

        return await base.Handle(request);
    }

    protected override void UpdateEntity(Drug entity, UpdateDrugRequest request)
    {
        entity.BrandName = request.BrandName;
        entity.GenericName = request.GenericName;
        entity.ClassId = request.ClassId;
        entity.Price = request.Price;
    }
}

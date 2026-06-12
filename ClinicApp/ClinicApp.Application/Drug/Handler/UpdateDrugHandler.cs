using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class UpdateDrugHandler
{
    private readonly IDrugRepository _drugRepository;
    private readonly IDrugClassRepository _drugClassRepository;
    private readonly IValidator<UpdateDrugRequest> _validator;

    public UpdateDrugHandler(IDrugRepository drugRepository, IDrugClassRepository drugClassRepository, IValidator<UpdateDrugRequest> validator)
    {
        _drugRepository = drugRepository;
        _drugClassRepository = drugClassRepository;
        _validator = validator;
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

        var validation = _validator.Validate(request);

        if(!validation.IsValid)
        {
            return new ValidationResponseModel(validation.ToDictionary());
        }

        entity.BrandName = request.BrandName;
        entity.GenericName = request.GenericName;
        entity.ClassId = request.ClassId;
        entity.Price = request.Price;

        await _drugRepository.UpdateAsync(entity);

        return new ValidationResponseModel();
    }
}

using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class AddDrugHandler : AddEntityHandler<Drug, AddDrugRequest>
{
    private readonly IDrugRepository _drugRepository;
    private readonly IDrugClassRepository _drugClassRepository;

    public AddDrugHandler(IDrugRepository drugRepository, IDrugClassRepository drugClassRepository, IValidator<AddDrugRequest> validator)
        : base(drugRepository, validator, new DrugModelMapper())
    {
        _drugRepository = drugRepository;
        _drugClassRepository = drugClassRepository;
    }

    public override async Task<ValidationResponseModel> Handle(AddDrugRequest request)
    {
        var drugClass = await _drugClassRepository.GetByIdAsync(request.ClassId);

        if(drugClass is null)
        {
            return new ValidationResponseModel("Drug class not found");
        }

        var drugs = await _drugRepository.GetAsync(new GetDrugRequest { BrandNameExact = request.BrandName });

        if(drugs.Any())
        {
            return new ValidationResponseModel("Name taken");
        }

        return await base.Handle(request);
    }
}

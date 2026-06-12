using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class AddDrugHandler
{
    private readonly IValidator<AddDrugRequest> _validator;
    private readonly IDrugRepository _drugRepository;
    private readonly IDrugClassRepository _drugClassRepository;
    private readonly DrugModelMapper _mapper;

    public AddDrugHandler(IDrugRepository drugRepository, IDrugClassRepository drugClassRepository, IValidator<AddDrugRequest> validator)
    {
        _validator = validator;
        _drugRepository = drugRepository;
        _drugClassRepository = drugClassRepository;
        _mapper = new DrugModelMapper();
    }

    public async Task<ValidationResponseModel> Handle(AddDrugRequest request)
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

        var validation = _validator.Validate(request);

        if(!validation.IsValid)
        {
            return new ValidationResponseModel(validation.ToDictionary());
        }

        var entity = _mapper.MapRequestToEntity(request);

        await _drugRepository.AddAsync(entity);

        return new ValidationResponseModel();
    }
}

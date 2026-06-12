using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class AddDrugClassHandler
{
    private readonly IDrugClassRepository _repository;
    private readonly IValidator<AddDrugClassRequest> _validator;
    private readonly DrugClassModelMapper _mapper;

    public AddDrugClassHandler(IDrugClassRepository drugClassRepository, IValidator<AddDrugClassRequest> validator)
    {
        _repository = drugClassRepository;
        _validator = validator;
        _mapper = new DrugClassModelMapper();
    }

    public async Task<ValidationResponseModel> Handle(AddDrugClassRequest request)
    {
        var classes = await _repository.GetAsync(new GetDrugClassRequest
        {
            Name = request.Name,
        });

        if(classes.Any())
        {
            return new ValidationResponseModel("Name already taken");
        }

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

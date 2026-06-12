using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class AddDrugClassHandler : AddEntityHandler<DrugClass, AddDrugClassRequest>
{
    private readonly IDrugClassRepository _drugClassRepository;

    public AddDrugClassHandler(IDrugClassRepository repository, IValidator<AddDrugClassRequest> validator) 
        : base(repository, validator, new DrugClassModelMapper())
    {
        _drugClassRepository = repository;
    }

    public override async Task<ValidationResponseModel> Handle(AddDrugClassRequest request)
    {
        var classes = await _drugClassRepository.GetAsync(new GetDrugClassRequest
        {
            Name = request.Name,
        });

        if(classes.Any())
        {
            return new ValidationResponseModel("Name already taken");
        }

        return await base.Handle(request);
    }
}

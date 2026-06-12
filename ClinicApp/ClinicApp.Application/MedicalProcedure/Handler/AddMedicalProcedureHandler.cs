using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class AddMedicalProcedureHandler : AddEntityHandler<MedicalProcedure, AddMedicalProcedureRequest>
{
    private readonly IMedicalProcedureRepository _medicalProcedureRepository;

    public AddMedicalProcedureHandler(IMedicalProcedureRepository repository,
        IValidator<AddMedicalProcedureRequest> validator) : base(repository, validator, new MedicalProcedureModelMapper())
    {
        _medicalProcedureRepository = repository;
    }

    public override async Task<ValidationResponseModel> Handle(AddMedicalProcedureRequest request)
    {
        var sameNameEntities = await _medicalProcedureRepository.GetAsync(new GetMedicalProcedureRequest
        {
            NameExact = request.Name
        });

        if(sameNameEntities.Any())
        {
            return new ValidationResponseModel("Name already taken");
        }

        return await base.Handle(request);
    }
}

using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class UpdateMedicalProcedureHandler : UpdateEntityHandler<MedicalProcedure, UpdateMedicalProcedureRequest>
{
    private readonly IMedicalProcedureRepository _medicalProcedureRepository;

    public UpdateMedicalProcedureHandler(IMedicalProcedureRepository repository, IValidator<UpdateMedicalProcedureRequest> validator)
        : base(repository, validator)
    {
        _medicalProcedureRepository = repository;
    }

    public override async Task<ValidationResponseModel> Handle(UpdateMedicalProcedureRequest request)
    {
        var sameNameEntities = await _medicalProcedureRepository.GetAsync(new GetMedicalProcedureRequest
        {
            NameExact = request.Name
        });

        if(sameNameEntities.Any(p => p.Id != request.Id))
        {
            return new ValidationResponseModel("Name taken");
        }

        return await base.Handle(request);
    }

    protected override void UpdateEntity(MedicalProcedure entity, UpdateMedicalProcedureRequest request)
    {
        entity.Name = request.Name;
        entity.Cost = request.Cost;
        entity.Description = request.Description;
    }
}

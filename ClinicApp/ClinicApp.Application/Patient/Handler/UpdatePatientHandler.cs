using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Update;
using FluentValidation;

namespace ClinicApp.Application.Handler;

public class UpdatePatientHandler : UpdateEntityHandler<Patient, UpdatePatientRequest>
{
    public UpdatePatientHandler(IPatientRepository repository, IValidator<UpdatePatientRequest> validator)
        : base(repository, validator)
    {
    }

    protected override void UpdateEntity(Patient entity, UpdatePatientRequest request)
    {
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;
        entity.Address = request.Address;
        entity.PostalCode = request.PostalCode;
        entity.City = request.City;
    }
}

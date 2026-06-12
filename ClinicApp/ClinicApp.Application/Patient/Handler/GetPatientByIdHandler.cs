using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Handler;

public record GetPatientByIdRequest(Guid Id) : GetEntityByIdRequest(Id);

public class GetPatientByIdHandler : GetEntityByIdHandler<Patient, PatientModel, GetPatientByIdRequest>
{
    public GetPatientByIdHandler(IPatientRepository repository) : base(repository, new PatientModelMapper())
    {
    }
}

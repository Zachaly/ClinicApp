using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;

namespace ClinicApp.Application.Handler;

public record DeletePatientByIdRequest(Guid Id) : DeleteEntityByIdRequest(Id);

public class DeletePatientByIdHandler : DeleteEntityByIdHandler<Patient, DeletePatientByIdRequest>
{
    public DeletePatientByIdHandler(IPatientRepository repository) : base(repository)
    {
    }
}

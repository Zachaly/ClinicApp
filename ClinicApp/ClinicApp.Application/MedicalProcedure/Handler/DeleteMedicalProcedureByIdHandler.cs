using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Handler;

public record DeleteMedicalProcedureByIdRequest(Guid Id) : DeleteEntityByIdRequest(Id);

public class DeleteMedicalProcedureByIdHandler : DeleteEntityByIdHandler<MedicalProcedure, DeleteMedicalProcedureByIdRequest>
{
    public DeleteMedicalProcedureByIdHandler(IMedicalProcedureRepository repository) : base(repository)
    {
    }
}

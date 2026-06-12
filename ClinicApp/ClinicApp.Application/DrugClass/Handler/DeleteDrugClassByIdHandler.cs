using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Handler;

public record DeleteDrugClassByIdRequest(Guid Id) : DeleteEntityByIdRequest(Id);

public class DeleteDrugClassByIdHandler : DeleteEntityByIdHandler<DrugClass, DeleteDrugClassByIdRequest>
{
    public DeleteDrugClassByIdHandler(IDrugClassRepository repository) : base(repository)
    {
    }
}

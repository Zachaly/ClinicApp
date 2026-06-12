using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Handler;

public record DeleteDrugByIdRequest(Guid Id) : DeleteEntityByIdRequest(Id);

public class DeleteDrugByIdHandler : DeleteEntityByIdHandler<Drug, DeleteDrugByIdRequest>
{
    public DeleteDrugByIdHandler(IDrugRepository repository) : base(repository)
    {
    }
}

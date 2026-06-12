using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.Handler;

public record GetDrugByIdRequest(Guid Id) : GetEntityByIdRequest(Id);

public class GetDrugByIdHandler : GetEntityByIdHandler<Drug, DrugModel, GetDrugByIdRequest>
{
    public GetDrugByIdHandler(IDrugRepository repository) : base(repository, new DrugModelMapper(), ["Class"])
    {
    }
}

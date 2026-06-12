using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetDrugHandler : GetEntityHandler<Drug, GetDrugRequest, DrugModel>
{
    public GetDrugHandler(IDrugRepository repository) : base(repository, new DrugModelMapper(), ["Class"])
    {
    }
}

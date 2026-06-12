using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetDrugClassCountRequest : GetDrugClassRequest;

public class GetDrugClassCountHandler : GetCountHandler<DrugClass, GetDrugClassRequest, GetDrugClassCountRequest>
{
    public GetDrugClassCountHandler(IDrugClassRepository repository) : base(repository)
    {
    }
}

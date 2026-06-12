using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetDrugCountRequest : GetDrugRequest;

public class GetDrugCountHandler : GetCountHandler<Drug, GetDrugRequest, GetDrugCountRequest>
{
    public GetDrugCountHandler(IDrugRepository repository) : base(repository)
    { 
    }
}

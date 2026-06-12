using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetPatientCountRequest : GetPatientRequest;

public class GetPatientCountHandler : GetCountHandler<Patient, GetPatientRequest, GetPatientCountRequest>
{
    public GetPatientCountHandler(IPatientRepository repository) : base(repository)
    {
    }
}

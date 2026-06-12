using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetPatientHandler : GetEntityHandler<Patient, GetPatientRequest, PatientModel>
{
    public GetPatientHandler(IPatientRepository repository) : base(repository, new PatientModelMapper())
    {
    }
}

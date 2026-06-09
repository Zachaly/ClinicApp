using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;

namespace ClinicApp.Domain.Repository;

public interface IPatientRepository : IRepository<Patient, GetPatientRequest>
{
}

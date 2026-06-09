using ClinicApp.Database;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;
using ClinicApp.Infrastructure.Repository.Base;

namespace ClinicApp.Infrastructure.Repository;

public class PatientRepository : RepositoryBase<Patient, GetPatientRequest>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

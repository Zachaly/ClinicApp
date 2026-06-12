using ClinicApp.Database;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Infrastructure.Repository.Base;

namespace ClinicApp.Infrastructure.Repository;

public class MedicalProcedureRepository : RepositoryBase<MedicalProcedure, GetMedicalProcedureRequest>, IMedicalProcedureRepository
{
    public MedicalProcedureRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

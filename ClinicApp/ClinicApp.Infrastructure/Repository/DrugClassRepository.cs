using ClinicApp.Database;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Infrastructure.Repository.Base;

namespace ClinicApp.Infrastructure.Repository;

public class DrugClassRepository : RepositoryBase<DrugClass, GetDrugClassRequest>, IDrugClassRepository
{
    public DrugClassRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}

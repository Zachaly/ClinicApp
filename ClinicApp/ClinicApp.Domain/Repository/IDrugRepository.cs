using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Domain.Repository;

public interface IDrugRepository : IRepository<Drug, GetDrugRequest>
{
}

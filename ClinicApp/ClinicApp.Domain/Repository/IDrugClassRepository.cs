using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Domain.Repository;

public interface IDrugClassRepository : IRepository<DrugClass, GetDrugClassRequest>
{
}

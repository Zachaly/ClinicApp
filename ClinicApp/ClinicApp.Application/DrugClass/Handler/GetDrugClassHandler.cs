using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Application.Handler;

public class GetDrugClassHandler : GetEntityHandler<DrugClass, GetDrugClassRequest, DrugClassModel>
{
    public GetDrugClassHandler(IDrugClassRepository repository) : base(repository, new DrugClassModelMapper())
    {
    }
}

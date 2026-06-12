using Riok.Mapperly.Abstractions;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Application.Model;
using ClinicApp.Application.Abstraction;

namespace ClinicApp.Application;

[Mapper]
public partial class DrugModelMapper : IModelMapper<Drug, DrugModel>, IRequestMapper<Drug, AddDrugRequest>
{
    public partial Drug MapRequestToEntity(AddDrugRequest request);
    public partial DrugModel MapEntityToModel(Drug entity);
}

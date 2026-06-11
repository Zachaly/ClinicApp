using Riok.Mapperly.Abstractions;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Application.Model;

namespace ClinicApp.Application;

[Mapper]
public partial class DrugModelMapper
{
    public partial Drug MapRequestToEntity(AddDrugRequest request);
    public partial DrugModel MapEntityToModel(Drug entity);
}

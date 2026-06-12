using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Entity;
using Riok.Mapperly.Abstractions;
using ClinicApp.Application.Model;
using ClinicApp.Application.Abstraction;

namespace ClinicApp.Application;

[Mapper]
public partial class DrugClassModelMapper : IRequestMapper<DrugClass, AddDrugClassRequest>, IModelMapper<DrugClass, DrugClassModel>
{
    public partial DrugClass MapRequestToEntity(AddDrugClassRequest request);
    public partial DrugClassModel MapEntityToModel(DrugClass entity);
}

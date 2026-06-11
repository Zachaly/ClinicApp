using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Entity;
using Riok.Mapperly.Abstractions;
using ClinicApp.Application.Model;

namespace ClinicApp.Application;

[Mapper]
public partial class DrugClassModelMapper
{
    public partial DrugClass MapRequestToEntity(AddDrugClassRequest request);
    public partial DrugClassModel MapEntityToModel(DrugClass entity);
}

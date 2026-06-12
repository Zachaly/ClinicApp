using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Add;
using Riok.Mapperly.Abstractions;
namespace ClinicApp.Application;

[Mapper]
public partial class MedicalProcedureModelMapper : IModelMapper<MedicalProcedure, MedicalProcedureModel>,
    IRequestMapper<MedicalProcedure, AddMedicalProcedureRequest>
{
    public partial MedicalProcedureModel MapEntityToModel(MedicalProcedure entity);
    public partial MedicalProcedure MapRequestToEntity(AddMedicalProcedureRequest request);
}

using ClinicApp.Application.Abstraction;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Add;
using Riok.Mapperly.Abstractions;

namespace ClinicApp.Application;

[Mapper]
public partial class PatientModelMapper : IModelMapper<Patient, PatientModel>, IRequestMapper<Patient, AddPatientRequest>
{
    public partial PatientModel MapEntityToModel(Patient patient);
    public partial Patient MapRequestToEntity(AddPatientRequest request);
}

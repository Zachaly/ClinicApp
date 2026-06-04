using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
using Riok.Mapperly.Abstractions;

namespace ClinicApp.Application;

[Mapper]
public partial class PatientModelMapper
{
    public partial PatientModel MapPatientToModel(Patient patient);
    public partial Patient MapRequestToEntity(AddPatientRequest request);
}

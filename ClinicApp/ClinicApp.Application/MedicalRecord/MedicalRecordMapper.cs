using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using Riok.Mapperly.Abstractions;
using ClinicApp.Domain.Request;
namespace ClinicApp.Application;

[Mapper]

public partial class MedicalRecordMapper
{
    [UserMapping]
    private List<string> MapClaims(List<UserClaim> claims)
        => claims.Select(c => c.ClaimValue).ToList();
    public partial MedicalRecord MapRequestToEntity(AddMedicalRecordRequest request);  // ← dodaj tę linię

}
using ClinicApp.Domain.Attribute;
using ClinicApp.Domain.Entity;

namespace ClinicApp.Domain.Request.Get;

public class GetMedicalProcedureRequest : PagedRequest
{
    [RequestField(ComparisonType = Enum.ComparisonType.StartsWith)]
    public string? Name { get; set; }
    [RequestField(Property = nameof(MedicalProcedure.Name))]
    public string? NameExact { get; set; }
}

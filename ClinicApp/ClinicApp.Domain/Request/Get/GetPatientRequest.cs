using ClinicApp.Domain.Attribute;

namespace ClinicApp.Domain.Request.Get;

public class GetPatientRequest : PagedRequest
{
    public string? PeselNumber { get; set; }
    [RequestField(ComparisonType = Enum.ComparisonType.StartsWith)]
    public string? LastName { get; set; }
}

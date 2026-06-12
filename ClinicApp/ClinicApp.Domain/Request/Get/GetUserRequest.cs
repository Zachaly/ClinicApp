using ClinicApp.Domain.Attribute;
using ClinicApp.Domain.Enum;
using ClinicApp.Domain.Request.Get;

namespace ClinicApp.Domain.Request;

public class GetUserRequest : PagedRequest
{
    [RequestField(ComparisonType = ComparisonType.StartsWith)]
    public string? LastName { get; set; }
    [RequestField(ComparisonType = ComparisonType.StartsWith)]
    public string? FirstName { get; set; }
    [RequestField(ComparisonType = ComparisonType.StartsWith)]
    public string? UserName { get; set; }
}

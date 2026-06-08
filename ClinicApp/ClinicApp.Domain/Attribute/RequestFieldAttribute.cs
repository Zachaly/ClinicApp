using ClinicApp.Domain.Enum;

namespace ClinicApp.Domain.Attribute;

public class RequestFieldAttribute : System.Attribute
{
    public string Property { get; set; }
    public ComparisonType ComparisonType { get; set; } = ComparisonType.Equal;
    public bool Skip { get; set; } = false;
}

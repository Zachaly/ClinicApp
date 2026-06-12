using ClinicApp.Domain.Attribute;
using ClinicApp.Domain.Entity;

namespace ClinicApp.Domain.Request.Get;

public class GetDrugRequest : PagedRequest
{
    [RequestField(ComparisonType = Enum.ComparisonType.StartsWith)]
    public string? BrandName { get; set; }
    [RequestField(ComparisonType = Enum.ComparisonType.StartsWith)]
    public string? GenericName { get; set; }
    public Guid? ClassId { get; set; }
    [RequestField(Property = nameof(Drug.BrandName))]
    public string? BrandNameExact { get; set; }
    [RequestField(Property = nameof(Drug.Price), ComparisonType = Enum.ComparisonType.LesserThan)]
    public decimal? MaxPrice { get; set; }
}

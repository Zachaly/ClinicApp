namespace ClinicApp.Domain.Request.Add;

public record AddDrugRequest
{
    public string BrandName { get; set; }
    public string GenericName { get; set; }
    public Guid ClassId { get; set; }
}

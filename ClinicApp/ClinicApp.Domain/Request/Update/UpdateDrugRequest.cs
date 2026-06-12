namespace ClinicApp.Domain.Request.Update;

public class UpdateDrugRequest
{
    public Guid Id { get; set; }
    public string BrandName { get; set; }
    public string GenericName { get; set; }
    public Guid ClassId { get; set; }
    public decimal Price { get; set; }
}

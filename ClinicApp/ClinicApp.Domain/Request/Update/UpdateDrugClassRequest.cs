namespace ClinicApp.Domain.Request.Update;

public record UpdateDrugClassRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

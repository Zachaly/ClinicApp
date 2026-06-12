namespace ClinicApp.Domain.Request.Update;

public record UpdateDrugClassRequest : IUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

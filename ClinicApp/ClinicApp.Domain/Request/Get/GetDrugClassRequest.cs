namespace ClinicApp.Domain.Request.Get;

public class GetDrugClassRequest : PagedRequest
{
    public string? Name { get; set; }
}

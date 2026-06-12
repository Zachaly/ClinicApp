namespace ClinicApp.Domain.Request.Update;

public record UpdatePatientRequest : IUpdateRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
}

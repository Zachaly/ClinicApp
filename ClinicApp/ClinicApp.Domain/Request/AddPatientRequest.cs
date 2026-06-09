namespace ClinicApp.Domain.Request;

public class AddPatientRequest
{
    public string PeselNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTimeOffset BirthDate { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
}

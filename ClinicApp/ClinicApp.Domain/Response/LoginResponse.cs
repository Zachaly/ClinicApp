namespace ClinicApp.Domain.Response;

public class LoginResponse : ResponseModel
{
    public Guid UserId { get; set; }
    public string AuthToken { get; set; }
}

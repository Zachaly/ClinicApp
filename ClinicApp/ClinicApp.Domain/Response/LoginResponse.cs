namespace ClinicApp.Domain.Response;

public class LoginResponse : ResponseModel
{
    public Guid UserId { get; set; }
    public string AuthToken { get; set; }
    public List<string> Claims { get; set; }

    public LoginResponse(string error) : base(error)
    {
        
    }

    public LoginResponse(Guid userId, string authToken, List<string> claims)
    {
        UserId = userId;
        AuthToken = authToken;
        Claims = claims;
    }
}

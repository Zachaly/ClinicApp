namespace ClinicApp.Infrastructure.Configuration;

public class AuthConfig
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int TokenLifetimeMinutes { get; set; }
}

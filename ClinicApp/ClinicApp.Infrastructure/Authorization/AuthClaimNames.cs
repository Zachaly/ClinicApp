namespace ClinicApp.Infrastructure.Authorization;

public static class AuthClaimNames 
{
    public const string Admin = "admin";
    public const string Doctor = "doctor";
    public const string Receptionist = "receptionist";

    public const string RoleClaim = "role";
}

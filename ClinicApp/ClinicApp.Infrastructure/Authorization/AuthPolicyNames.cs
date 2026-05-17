namespace ClinicApp.Infrastructure.Authorization;

public static class AuthPolicyNames
{
    public const string RequireAdmin = "Admin";
    public const string RequireDoctor = "Doctor";
    public const string RequireReceptionist = "Receptionist";

    public const string RequireAdminOrReceptionist = RequireAdmin + RequireReceptionist;
    public const string RequireAdminOrDoctor = RequireAdmin + RequireDoctor;
}

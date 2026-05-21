namespace ClinicApp.Domain.Entity;

public class UserClaim : IEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}

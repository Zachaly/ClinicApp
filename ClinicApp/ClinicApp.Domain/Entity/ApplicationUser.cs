using System.Security.Claims;

namespace ClinicApp.Domain.Entity;

public class ApplicationUser : IUser
{
    public Guid Id { get; set ; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

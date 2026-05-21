using ClinicApp.Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace ClinicApp.Database.Model;

public class DatabaseUser : IdentityUser<Guid>, IUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
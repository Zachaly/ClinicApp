using System.Security.Claims;

namespace ClinicApp.Domain.Entity;

public interface IUser : IEntity
{
    string UserName { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
}

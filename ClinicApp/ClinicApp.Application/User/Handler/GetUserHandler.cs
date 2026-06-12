using ClinicApp.Application.Abstraction;
using ClinicApp.Application.User.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;

namespace ClinicApp.Application.User.Handler;

public class GetUserHandler : GetEntityHandler<ApplicationUser, GetUserRequest, UserModel>
{
    public GetUserHandler(IUserRepository repository) : base(repository, new UserModelMapper(), ["Claims"])
    {
    }
}

using ClinicApp.Application.Abstraction;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;

namespace ClinicApp.Application.User.Handler;

public class GetUserCountRequest : GetUserRequest;

public class GetUserCountHandler : GetCountHandler<ApplicationUser, GetUserRequest, GetUserCountRequest>
{
    public GetUserCountHandler(IUserRepository repository) : base(repository)
    {
    }
}

using ClinicApp.Application.Abstraction;
using ClinicApp.Application.User.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;

namespace ClinicApp.Application.User.Handler;

public record GetUserByIdRequest(Guid Id) : GetEntityByIdRequest(Id);

public class GetUserByIdHandler : GetEntityByIdHandler<ApplicationUser, UserModel, GetUserByIdRequest>
{
    public GetUserByIdHandler(IUserRepository repository) : base(repository, new UserModelMapper(), ["Claims"])
    {
    }
}

using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;

namespace ClinicApp.Application.Authorization.Handler;

public record AddAdminClaimRequest(Guid Id) : AddUserClaimRequest(Id);

public class AddAdminClaimHandler : AddUserClaimHandler<AddAdminClaimRequest>
{
    public AddAdminClaimHandler(IUserService userService, IUserRepository userRepository) : base(userService, userRepository)
    {
    }

    protected override Task<ResponseModel> AddClaim(AddAdminClaimRequest request)
        => _userService.AddAdminClaim(request.Id);
}

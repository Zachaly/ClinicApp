using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;

namespace ClinicApp.Application.Authorization.Handler;

public record AddReceptionistClaimRequest(Guid Id) : AddUserClaimRequest(Id);

public class AddReceptionistClaimHandler : AddUserClaimHandler<AddReceptionistClaimRequest>
{
    public AddReceptionistClaimHandler(IUserService userService, IUserRepository userRepository) : base(userService, userRepository)
    {
    }

    protected override Task<ResponseModel> AddClaim(AddReceptionistClaimRequest request)
        => _userService.AddReceptionistClaim(request.Id);
}

using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;

namespace ClinicApp.Application.Authorization.Handler;

public record AddDoctorClaimRequest(Guid Id) : AddUserClaimRequest(Id);

public class AddDoctorClaimHandler : AddUserClaimHandler<AddDoctorClaimRequest>
{
    public AddDoctorClaimHandler(IUserService userService, IUserRepository userRepository) : base(userService, userRepository)
    {
    }

    protected override Task<ResponseModel> AddClaim(AddDoctorClaimRequest request)
        => _userService.AddDoctorClaim(request.Id);
}

using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;

namespace ClinicApp.Application.Authorization.Handler;

public record AddUserClaimRequest(Guid Id);

public abstract class AddUserClaimHandler<TRequest> where TRequest : AddUserClaimRequest
{
    protected readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    protected AddUserClaimHandler(IUserService userService, IUserRepository userRepository)
    {
        _userService = userService;
        _userRepository = userRepository;
    }

    public async Task<ResponseModel> Handle(TRequest request)
    {
        if((await _userRepository.GetByIdAsync(request.Id)) is null)
        {
            return new ResponseModel("User not found");
        }

        return await AddClaim(request);
    }

    protected abstract Task<ResponseModel> AddClaim(TRequest request);
}

using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;

namespace ClinicApp.Application.Authorization.Handler;

public record LoginRequest(string Login, string Password);

public class LoginHandler
{
    private readonly IUserService _userService;

    public LoginHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<LoginResponse> Handle(LoginRequest request)
    {
        var userId = await _userService.CheckPasswordAsync(request.Login, request.Password);

        if(userId == Guid.Empty)
        {
            return new LoginResponse("Invalid login or password");
        }

        var claims = await _userService.GetClaimsAsync(userId);

        var token = await _userService.GenerateTokenAsync(userId, claims);

        return new LoginResponse(userId, token, claims.Select(x => x.Value).ToList());
    }
}

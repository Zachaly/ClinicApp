using ClinicApp.Application.Authorization.Handler;
using ClinicApp.Domain.Service;
using NSubstitute;
using System.Security.Claims;

namespace ClinicApp.Tests.Unit.Handler.Authorization;

public class LoginHandlerTests
{
    private readonly IUserService _userService;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _handler = new LoginHandler(_userService);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsTokenIdAndClaims()
    {
        var request = new LoginRequest("login", "password");

        var userId = Guid.NewGuid();

        _userService.CheckPasswordAsync(request.Login, request.Password).Returns(userId);

        var claims = new List<Claim>() { new Claim("role", "claim") };

        _userService.GetClaimsAsync(userId).Returns(claims);

        var token = "token";

        _userService.GenerateTokenAsync(userId, claims).Returns(token);

        var response = await _handler.Handle(request);

        Assert.True(response.IsSuccess);
        Assert.Equal(token, response.AuthToken);
        Assert.Equal(userId, response.UserId);
        Assert.Equal(claims.Select(c => c.Value), response.Claims);
    }

    [Fact]
    public async Task Handle_InvalidCredentials_ReturnsFailure()
    {
        var request = new LoginRequest("login", "password");

        _userService.CheckPasswordAsync(request.Login, request.Password).Returns(Guid.Empty);

        var response = await _handler.Handle(request);

        Assert.False(response.IsSuccess);
        Assert.NotEmpty(response.Error);
    }

}

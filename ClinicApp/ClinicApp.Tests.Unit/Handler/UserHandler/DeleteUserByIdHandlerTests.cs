using ClinicApp.Application.User.Handler;
using ClinicApp.Domain.Service;
using NSubstitute;

namespace ClinicApp.Tests.Unit.Handler.User;

public class DeleteUserByIdHandlerTests
{
    private readonly IUserService _userService;
    private readonly DeleteUserByIdHandler _handler;

    public DeleteUserByIdHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _handler = new DeleteUserByIdHandler(_userService);
    }

    [Fact]
    public async Task Handle_DeletesUser_ReturnsSuccess()
    {
        var request = new DeleteUserByIdRequest(Guid.NewGuid());

        var response = await _handler.Handle(request);

        await _userService.Received(1).DeleteUserAsync(request.Id);

        Assert.True(response.IsSuccess);
    } 
}

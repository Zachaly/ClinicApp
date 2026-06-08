using ClinicApp.Application.User.Handler;
using ClinicApp.Application.User.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ClinicApp.Tests.Unit.Handler.User;

public class GetUserByIdHandlerTests
{
    private readonly IUserRepository _repository;
    private readonly GetUserByIdHandler _handler;

    public GetUserByIdHandlerTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _handler = new GetUserByIdHandler(_repository);
    }

    [Fact]
    public async Task Handle_UserFound_ReturnsUserModel()
    {
        var request = new GetUserByIdRequest(Guid.NewGuid());
        var user = new ApplicationUser { Claims = [] };

        _repository.GetByIdAsync(request.Id, Arg.Is<List<string>>(l => l.Contains("Claims"))).Returns(user);

        var response = await _handler.Handle(request);

        Assert.NotNull(response);
        Assert.IsType<UserModel>(response);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNull()
    {
        var request = new GetUserByIdRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id, Arg.Is<List<string>>(l => l.Contains("Claims"))).ReturnsNull();

        var response = await _handler.Handle(request);

        Assert.Null(response);
    }
}

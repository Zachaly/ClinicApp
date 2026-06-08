using ClinicApp.Application.User.Handler;
using ClinicApp.Application.User.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;
using NSubstitute;

namespace ClinicApp.Tests.Unit.Handler.User;

public class GetUserHandlerTests
{
    private readonly IUserRepository _repository;
    private readonly GetUserHandler _handler;

    public GetUserHandlerTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _handler = new GetUserHandler(_repository);
    }

    [Fact]
    public async Task Handle_ReturnsListOfModels()
    {
        var request = new GetUserRequest();

        List<ApplicationUser> users = [new ApplicationUser { Claims = [] }];

        _repository.GetAsync(request, Arg.Is<List<string>>(l => l.Contains("Claims"))).Returns(users);

        var response = await _handler.Handle(request);

        Assert.All(response, r => Assert.IsType<UserModel>(r));
    }
}

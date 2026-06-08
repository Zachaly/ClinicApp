using ClinicApp.Application.User.Handler;
using ClinicApp.Domain.Repository;
using NSubstitute;

namespace ClinicApp.Tests.Unit.Handler.User;

public class GetUserCountHandlerTests
{
    private readonly IUserRepository _repository;
    private readonly GetUserCountHandler _handler;

    public GetUserCountHandlerTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _handler = new GetUserCountHandler(_repository);
    }

    [Fact]
    public async Task Handle_ReturnsProperCount()
    {
        var request = new GetUserCountRequest();
        var count = 20;

        _repository.GetCountAsync(request).Returns(count);

        var response = await _handler.Handle(request);

        Assert.Equal(count, response);
    }
}

using ClinicApp.Application.Handler;
using ClinicApp.Domain.Repository;
using NSubstitute;

namespace ClinicApp.Tests.Unit.Handler.DrugClassHandler;

public class GetDrugClassCountHandlerTests
{
    private readonly IDrugClassRepository _repository;
    private readonly GetDrugClassCountHandler _handler;

    public GetDrugClassCountHandlerTests()
    {
        _repository = Substitute.For<IDrugClassRepository>();
        _handler = new GetDrugClassCountHandler(_repository);
    }

    [Fact]
    public async Task Handle_ReturnsCount()
    {
        var count = 20;

        var request = new GetDrugClassCountRequest();

        _repository.GetCountAsync(request).Returns(count);

        var result = await _handler.Handle(request);

        Assert.Equal(count, result);
    }
}

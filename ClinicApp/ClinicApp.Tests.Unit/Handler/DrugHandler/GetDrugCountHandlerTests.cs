using ClinicApp.Application.Handler;
using ClinicApp.Domain.Repository;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Handler.DrugHandler;

public class GetDrugCountHandlerTests
{
    private readonly IDrugRepository _repository;
    private readonly GetDrugCountHandler _handler;

    public GetDrugCountHandlerTests()
    {
        _repository = Substitute.For<IDrugRepository>();
        _handler = new GetDrugCountHandler(_repository);
    }

    [Fact]
    public async Task Handle_ReturnsCountOfDrugs()
    {
        var request = new GetDrugCountRequest();
        var count = 20;

        _repository.GetCountAsync(request).Returns(count);

        var result = await _handler.Handle(request);

        Assert.Equal(count, result);
    }
}

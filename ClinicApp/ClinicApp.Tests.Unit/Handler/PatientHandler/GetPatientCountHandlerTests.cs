using ClinicApp.Application.Handler;
using ClinicApp.Domain.Repository;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Handler.PatientHandler;

public class GetPatientCountHandlerTests
{
    private readonly IPatientRepository _repository;
    private readonly GetPatientCountHandler _handler;

    public GetPatientCountHandlerTests()
    {
        _repository = Substitute.For<IPatientRepository>();
        _handler = new GetPatientCountHandler(_repository);
    }

    [Fact]
    public async Task Handle_ReturnsResultFromRepository()
    {
        var request = new GetPatientCountRequest();

        var count = 20;

        _repository.GetCountAsync(request).Returns(count);

        var result = await _handler.Handle(request);

        Assert.Equal(count, result);
    }
}

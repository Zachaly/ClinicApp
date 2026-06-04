using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Handler.PatientHandler;

public class GetPatientByIdHandlerTests
{
    private readonly IPatientRepository _repository;
    private readonly GetPatientByIdHandler _handler;

    public GetPatientByIdHandlerTests()
    {
        _repository = Substitute.For<IPatientRepository>();
        _handler = new GetPatientByIdHandler(_repository);
    }

    [Fact]
    public async Task Handle_EntityFound_ReturnsModel()
    {
        var entity = new Patient
        {
            Id = Guid.NewGuid()
        };

        var request = new GetPatientByIdRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).Returns(entity);

        var result = await _handler.Handle(request);

        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public async Task Handle_EntityNotFound_ReturnsNull()
    {
        var request = new GetPatientByIdRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.Null(result);
    }
}

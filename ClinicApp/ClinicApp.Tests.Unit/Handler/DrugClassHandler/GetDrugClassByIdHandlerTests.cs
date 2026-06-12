using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Handler.DrugClassHandler;

public class GetDrugClassByIdHandlerTests
{
    private readonly IDrugClassRepository _repository;
    private readonly GetDrugClassByIdHandler _handler;

    public GetDrugClassByIdHandlerTests()
    {
        _repository = Substitute.For<IDrugClassRepository>();
        _handler = new GetDrugClassByIdHandler(_repository);
    }

    [Fact]
    public async Task Handle_EntityFound_ReturnsModel() 
    {
        var request = new GetDrugClassByIdRequest(Guid.NewGuid());
        var entity = new DrugClass
        {
            Name = "n",
            Id = Guid.NewGuid()
        };

        _repository.GetByIdAsync(request.Id, Arg.Any<List<string>>()).Returns(entity);

        var result = await _handler.Handle(request);

        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public async Task Handle_EntityNotFound_ReturnsNull()
    {
        var request = new GetDrugClassByIdRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.Null(result);
    }
}

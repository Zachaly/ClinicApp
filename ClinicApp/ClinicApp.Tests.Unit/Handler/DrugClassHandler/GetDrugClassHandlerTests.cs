using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Handler.DrugClassHandler;

public class GetDrugClassHandlerTests
{
    private readonly IDrugClassRepository _repository;
    private readonly GetDrugClassHandler _handler;

    public GetDrugClassHandlerTests()
    {
        _repository = Substitute.For<IDrugClassRepository>();
        _handler = new GetDrugClassHandler(_repository);
    }

    [Fact]
    public async Task Handle_ReturnsListOfModels()
    {
        var request = new GetDrugClassRequest();

        var entities = new List<DrugClass>
        {
            new DrugClass { Id = Guid.NewGuid() },
            new DrugClass { Id = Guid.NewGuid() }
        };

        _repository.GetAsync(request).Returns(entities);

        var result = await _handler.Handle(request);

        Assert.Equivalent(entities.Select(x => x.Id), result.Select(x => x.Id));
    }
}

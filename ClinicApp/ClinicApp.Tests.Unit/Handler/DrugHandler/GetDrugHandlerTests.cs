using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using NSubstitute;

namespace ClinicApp.Tests.Unit.Handler.DrugHandler;

public class GetDrugHandlerTests
{
    private readonly IDrugRepository _repository;
    private readonly GetDrugHandler _handler;

    public GetDrugHandlerTests()
    {
        _repository = Substitute.For<IDrugRepository>();
        _handler = new GetDrugHandler(_repository);
    }

    [Fact]
    public async Task Handle_ReturnsListOfModels()
    {
        var request = new GetDrugRequest();

        var entities = new List<Drug>
        {
            new Drug { Id = Guid.NewGuid(), Class = new DrugClass()},
            new Drug { Id = Guid.NewGuid(), Class = new DrugClass()}
        };

        _repository.GetAsync(request, Arg.Is<List<string>>(l => l.Contains("Class"))).Returns(entities);

        var result = await _handler.Handle(request);

        Assert.Equivalent(entities.Select(x => x.Id), result.Select(x => x.Id));
    }
}

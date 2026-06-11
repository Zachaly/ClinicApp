using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ClinicApp.Tests.Unit.Handler.DrugHandler;

public class GetDrugByIdHandlerTests
{
    private readonly IDrugRepository _repository;
    private readonly GetDrugByIdHandler _handler;

    public GetDrugByIdHandlerTests()
    {
        _repository = Substitute.For<IDrugRepository>();
        _handler = new GetDrugByIdHandler(_repository);
    }

    [Fact]
    public async Task Handle_EntityFound_ReturnsModel()
    {
        var request = new GetDrugByIdRequest(Guid.NewGuid());
        var entity = new Drug
        {
            Id = Guid.NewGuid(),
            Class = new DrugClass()
        };

        _repository.GetByIdAsync(request.Id).Returns(entity);

        var result = await _handler.Handle(request);

        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public async Task Handle_EntityNotFound_ReturnsNull()
    {
        var request = new GetDrugByIdRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.Null(result);
    }
}

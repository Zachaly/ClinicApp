using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ClinicApp.Tests.Unit.Handler.DrugHandler;

public class DeleteDrugByIdHandlerTests
{
    private readonly IDrugRepository _repository;
    private readonly DeleteDrugByIdHandler _handler;

    public DeleteDrugByIdHandlerTests()
    {
        _repository = Substitute.For<IDrugRepository>();
        _handler = new DeleteDrugByIdHandler(_repository);
    }

    [Fact]
    public async Task Handle_EntityFound_DeletesEntity()
    {
        var request = new DeleteDrugByIdRequest(Guid.NewGuid());

        var entity = new Drug();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        var result = await _handler.Handle(request);

        await _repository.Received(1).DeleteAsync(entity);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_EntityDoesNotExist_ReturnsSuccess()
    {
        var request = new DeleteDrugByIdRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.True(result.IsSuccess);
    }
}

using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ClinicApp.Tests.Unit.Handler.DrugClassHandler;

public class DeleteDrugClassByIdHandlerTests
{
    private readonly IDrugClassRepository _repository;
    private readonly DeleteDrugClassByIdHandler _handler;

    public DeleteDrugClassByIdHandlerTests()
    {
        _repository = Substitute.For<IDrugClassRepository>();
        _handler = new DeleteDrugClassByIdHandler(_repository);
    }

    [Fact]
    public async Task Handle_EntityFound_DeletesEntity()
    {
        var request = new DeleteDrugClassByIdRequest(Guid.NewGuid());

        var entity = new DrugClass();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        var result = await _handler.Handle(request);

        await _repository.Received(1).DeleteAsync(entity);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_EntityNotFound_ReturnsSuccess()
    {
        var request = new DeleteDrugClassByIdRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.True(result.IsSuccess);
    }
}

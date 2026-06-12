using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ClinicApp.Tests.Unit.Handler.PatientHandler;

public class DeletePatientByIdHandlerTests
{
    private readonly IPatientRepository _repository;
    private readonly DeletePatientByIdHandler _handler;

    public DeletePatientByIdHandlerTests()
    {
        _repository = Substitute.For<IPatientRepository>();
        _handler = new DeletePatientByIdHandler(_repository);
    }

    [Fact]
    public async Task Handle_EntityNotFound_EndsEarlyAndReturnsSuccess()
    {
        var request = new DeletePatientByIdRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_EntityFound_DeletesEntityAndReturnsSuccess()
    {
        var request = new DeletePatientByIdRequest(Guid.NewGuid());

        var entity = new Patient();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        var result = await _handler.Handle(request);

        await _repository.Received(1).DeleteAsync(entity);

        Assert.True(result.IsSuccess);
    }
}

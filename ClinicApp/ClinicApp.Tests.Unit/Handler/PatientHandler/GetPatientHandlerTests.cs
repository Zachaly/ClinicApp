using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using NSubstitute;

namespace ClinicApp.Tests.Unit.Handler.PatientHandler;

public class GetPatientHandlerTests
{
    private readonly IPatientRepository _repository;
    private readonly GetPatientHandler _handler;

    public GetPatientHandlerTests()
    {
        _repository = Substitute.For<IPatientRepository>();
        _handler = new GetPatientHandler(_repository);
    }

    [Fact]
    public async Task Handle_ReturnsListOfPatientModels()
    {
        var entities = new List<Patient>
        {
            new Patient { Id = Guid.NewGuid() }
        };

        var request = new GetPatientRequest();

        _repository.GetAsync(request).Returns(entities);

        var result = await _handler.Handle(request);

        Assert.Equivalent(entities.Select(x => x.Id), result.Select(x => x.Id));
    }
}

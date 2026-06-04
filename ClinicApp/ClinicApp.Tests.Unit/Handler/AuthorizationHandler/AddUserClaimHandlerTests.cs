using ClinicApp.Application.Authorization.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ClinicApp.Tests.Unit.Handler.Authorization;

public class AddUserClaimHandlerTests
{
    private readonly IUserRepository _repository;
    private readonly IUserService _service;

    public AddUserClaimHandlerTests()
    {
        _repository = Substitute.For<IUserRepository>();
        _service = Substitute.For<IUserService>();
    }

    [Fact]
    public async Task AddAdminClaimHandler_Handle_UserFound_ReturnsServiceResponse()
    {
        var request = new AddAdminClaimRequest(Guid.NewGuid());

        var user = new ApplicationUser();

        _repository.GetByIdAsync(request.Id).Returns(user);

        var serviceResponse = new ResponseModel();

        _service.AddAdminClaim(request.Id).Returns(serviceResponse);

        var handler = new AddAdminClaimHandler(_service, _repository);

        var response = await handler.Handle(request);

        Assert.Equal(serviceResponse, response);
    }

    [Fact]
    public async Task AddAdminClaimHandler_Handle_UserNotFound_ReturnsFailure()
    {
        var request = new AddAdminClaimRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var handler = new AddAdminClaimHandler(_service, _repository);

        var response = await handler.Handle(request);

        Assert.False(response.IsSuccess);
    }

    [Fact]
    public async Task AddDoctorClaimHandler_Handle_UserFound_ReturnsServiceResponse()
    {
        var request = new AddDoctorClaimRequest(Guid.NewGuid());

        var user = new ApplicationUser();

        _repository.GetByIdAsync(request.Id).Returns(user);

        var serviceResponse = new ResponseModel();

        _service.AddDoctorClaim(request.Id).Returns(serviceResponse);

        var handler = new AddDoctorClaimHandler(_service, _repository);

        var response = await handler.Handle(request);

        Assert.Equal(serviceResponse, response);
    }

    [Fact]
    public async Task AddDoctorClaimHandler_Handle_UserNotFound_ReturnsFailure()
    {
        var request = new AddDoctorClaimRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var handler = new AddDoctorClaimHandler(_service, _repository);

        var response = await handler.Handle(request);

        Assert.False(response.IsSuccess);
    }

    [Fact]
    public async Task AddReceptionistClaimHandler_Handle_UserFound_ReturnsServiceResponse()
    {
        var request = new AddReceptionistClaimRequest(Guid.NewGuid());

        var user = new ApplicationUser();

        _repository.GetByIdAsync(request.Id).Returns(user);

        var serviceResponse = new ResponseModel();

        _service.AddReceptionistClaim(request.Id).Returns(serviceResponse);

        var handler = new AddReceptionistClaimHandler(_service, _repository);

        var response = await handler.Handle(request);

        Assert.Equal(serviceResponse, response);
    }

    [Fact]
    public async Task AddReceptionistClaimHandler_Handle_UserNotFound_ReturnsFailure()
    {
        var request = new AddReceptionistClaimRequest(Guid.NewGuid());

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var handler = new AddReceptionistClaimHandler(_service, _repository);

        var response = await handler.Handle(request);

        Assert.False(response.IsSuccess);
    }
}

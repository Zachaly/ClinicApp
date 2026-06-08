using ClinicApp.Application.User.Handler;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace ClinicApp.Tests.Unit.Handler.User;

public class CreateUserHandlerTests
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _validator;

    public CreateUserHandlerTests()
    {
        _userService = Substitute.For<IUserService>();
        _validator = Substitute.For<IValidator<CreateUserRequest>>();
    }

    [Fact]
    public async Task CreateAdminUserHandler_Handle_ValidRequest_ReturnsSuccess()
    {
        var request = new CreateAdminUserRequest();

        _validator.Validate(request).Returns(new ValidationResult());

        var serviceResponse = new ValidationResponseModel();

        _userService.CreateAdminUserAsync(request).Returns(serviceResponse);

        var handler = new CreateAdminUserHandler(_userService, _validator);

        var response = await handler.Handle(request);

        Assert.Equal(serviceResponse, response);
    }

    [Fact]
    public async Task CreateAdminUserHandler_Handle_InvalidRequest_ReturnsFailure()
    {
        var request = new CreateAdminUserRequest();

        _validator.Validate(request).Returns(new ValidationResult([new ValidationFailure("prop", "err")]));

        var handler = new CreateAdminUserHandler(_userService, _validator);

        var response = await handler.Handle(request);

        Assert.False(response.IsSuccess);
        Assert.NotEmpty(response.ValidationErrors);
    }

    [Fact]
    public async Task CreateDoctorUserHandler_Handle_ValidRequest_ReturnsSuccess()
    {
        var request = new CreateDoctorUserRequest();

        _validator.Validate(request).Returns(new ValidationResult());

        var serviceResponse = new ValidationResponseModel();

        _userService.CreateDoctorUserAsync(request).Returns(serviceResponse);

        var handler = new CreateDoctorUserHandler(_userService, _validator);

        var response = await handler.Handle(request);

        Assert.Equal(serviceResponse, response);
    }

    [Fact]
    public async Task CreateDoctorUserHandler_Handle_InvalidRequest_ReturnsFailure()
    {
        var request = new CreateDoctorUserRequest();

        _validator.Validate(request).Returns(new ValidationResult([new ValidationFailure("prop", "err")]));

        var handler = new CreateDoctorUserHandler(_userService, _validator);

        var response = await handler.Handle(request);

        Assert.False(response.IsSuccess);
        Assert.NotEmpty(response.ValidationErrors);
    }

    [Fact]
    public async Task CreateReceptionistUserHandler_Handle_ValidRequest_ReturnsSuccess()
    {
        var request = new CreateReceptionistUserRequest();

        _validator.Validate(request).Returns(new ValidationResult());

        var serviceResponse = new ValidationResponseModel();

        _userService.CreateReceptionistUserAsync(request).Returns(serviceResponse);

        var handler = new CreateReceptionistUserHandler(_userService, _validator);

        var response = await handler.Handle(request);

        Assert.Equal(serviceResponse, response);
    }

    [Fact]
    public async Task CreateReceptionistUserHandler_Handle_InvalidRequest_ReturnsFailure()
    {
        var request = new CreateReceptionistUserRequest();

        _validator.Validate(request).Returns(new ValidationResult([new ValidationFailure("prop", "err")]));

        var handler = new CreateReceptionistUserHandler(_userService, _validator);

        var response = await handler.Handle(request);

        Assert.False(response.IsSuccess);
        Assert.NotEmpty(response.ValidationErrors);
    }
}

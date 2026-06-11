using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Update;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ClinicApp.Tests.Unit.Handler.PatientHandler;

public class UpdatePatientHandlerTests
{
    private readonly IPatientRepository _repository;
    private readonly IValidator<UpdatePatientRequest> _validator;
    private readonly UpdatePatientHandler _handler;

    public UpdatePatientHandlerTests()
    {
        _repository = Substitute.For<IPatientRepository>();
        _validator = Substitute.For<IValidator<UpdatePatientRequest>>();
        _handler = new UpdatePatientHandler(_repository, _validator);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess()
    {
        var request = new UpdatePatientRequest
        {
            Id = Guid.NewGuid(),
            LastName = "lname",
            City = "city",
            FirstName = "fname",
            Address = "addr",
            PostalCode = "123"
        };

        var entity = new Patient();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        _validator.Validate(request).Returns(new ValidationResult());

        var result = await _handler.Handle(request);

        await _repository.Received(1).UpdateAsync(entity);

        Assert.True(result.IsSuccess);
        Assert.Equal(request.LastName, entity.LastName);
        Assert.Equal(request.City, entity.City);
        Assert.Equal(request.FirstName, entity.FirstName);
        Assert.Equal(request.Address, entity.Address);
        Assert.Equal(request.PostalCode, entity.PostalCode);
    }

    [Fact]
    public async Task Handle_EntityNotFound_ReturnsFailure()
    {
        var request = new UpdatePatientRequest
        {
            Id = Guid.NewGuid(),
        };

        var entity = new Patient();

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ValidationFailed_ReturnsFailure()
    {
        var request = new UpdatePatientRequest
        {
            Id = Guid.NewGuid(),
        };

        var entity = new Patient();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        _validator.Validate(request).Returns(new ValidationResult([new ValidationFailure("prop", "err")]));

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ValidationErrors);
    }
}

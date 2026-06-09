using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Handler.PatientHandler;

public class AddPatientHandlerTests
{
    private readonly IPatientRepository _repository;
    private readonly IValidator<AddPatientRequest> _validator;
    private readonly AddPatientHandler _handler;

    public AddPatientHandlerTests()
    {
        _repository = Substitute.For<IPatientRepository>();
        _validator = Substitute.For<IValidator<AddPatientRequest>>();
        _handler = new AddPatientHandler(_repository, _validator);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess()
    {
        var request = new AddPatientRequest
        {
            PeselNumber = "1"
        };

        _repository.GetAsync(Arg.Is<GetPatientRequest>(r => r.PeselNumber == request.PeselNumber))
            .Returns([]);

        _validator.Validate(request).Returns(new ValidationResult());

        var result = await _handler.Handle(request);

        await _repository.Received(1).AddAsync(Arg.Any<Patient>());

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_PeselTaken_ReturnsFailure()
    {
        var request = new AddPatientRequest
        {
            PeselNumber = "1"
        };

        _repository.GetAsync(Arg.Is<GetPatientRequest>(r => r.PeselNumber == request.PeselNumber))
            .Returns([new Patient()]);

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsFailure()
    {
        var request = new AddPatientRequest
        {
            PeselNumber = "1"
        };

        _repository.GetAsync(Arg.Is<GetPatientRequest>(r => r.PeselNumber == request.PeselNumber))
            .Returns([]);

        _validator.Validate(request).Returns(new ValidationResult([new ValidationFailure("prop", "err")]));

        var result = await _handler.Handle(request);
        
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ValidationErrors);
    }
}

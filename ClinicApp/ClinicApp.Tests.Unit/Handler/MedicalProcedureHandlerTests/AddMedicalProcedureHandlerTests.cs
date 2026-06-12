using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using FluentValidation;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicApp.Tests.Unit.Handler.MedicalProcedureHandlerTests;

public class AddMedicalProcedureHandlerTests
{
    private readonly IMedicalProcedureRepository _repository;
    private readonly IValidator<AddMedicalProcedureRequest> _validator;
    private readonly AddMedicalProcedureHandler _handler;

    public AddMedicalProcedureHandlerTests()
    {
        _repository = Substitute.For<IMedicalProcedureRepository>();
        _validator = Substitute.For<IValidator<AddMedicalProcedureRequest>>();

        _handler = new AddMedicalProcedureHandler(_repository, _validator);
    }

    [Fact]
    public async Task Handle_NameTaken_ReturnsFailure()
    {
        var request = new AddMedicalProcedureRequest
        {
            Name = "n"
        };

        _repository.GetAsync(Arg.Is<GetMedicalProcedureRequest>(r => r.NameExact == request.Name)).Returns([new MedicalProcedure()]);

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }
}

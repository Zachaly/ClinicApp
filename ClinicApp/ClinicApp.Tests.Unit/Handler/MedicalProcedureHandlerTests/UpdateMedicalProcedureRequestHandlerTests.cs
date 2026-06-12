using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace ClinicApp.Tests.Unit.Handler.MedicalProcedureHandlerTests;

public class UpdateMedicalProcedureRequestHandlerTests
{
    private readonly IMedicalProcedureRepository _repository;
    private readonly IValidator<UpdateMedicalProcedureRequest> _validator;
    private readonly UpdateMedicalProcedureHandler _handler;

    public UpdateMedicalProcedureRequestHandlerTests()
    {
        _repository = Substitute.For<IMedicalProcedureRepository>();
        _validator = Substitute.For<IValidator<UpdateMedicalProcedureRequest>>();

        _handler = new UpdateMedicalProcedureHandler(_repository, _validator);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesEntity()
    {
        var request = new UpdateMedicalProcedureRequest
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Description = "description",
            Cost = 123
        };

        var entity = new MedicalProcedure();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        _validator.Validate(request).Returns(new ValidationResult());

        _repository.GetAsync(Arg.Is<GetMedicalProcedureRequest>(r => r.NameExact == request.Name)).Returns([]);

        var result = await _handler.Handle(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(request.Name, entity.Name);
        Assert.Equal(request.Description, entity.Description);
        Assert.Equal(request.Cost, entity.Cost);
    }

    [Fact]
    public async Task Handle_NameTakenBySameEntity_UpdatesEntity()
    {
        var request = new UpdateMedicalProcedureRequest
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Description = "description",
            Cost = 123
        };

        var entity = new MedicalProcedure();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        _validator.Validate(request).Returns(new ValidationResult());

        _repository.GetAsync(Arg.Is<GetMedicalProcedureRequest>(r => r.NameExact == request.Name)).Returns([
            new MedicalProcedure { Id = request.Id }
            ]);

        var result = await _handler.Handle(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(request.Name, entity.Name);
        Assert.Equal(request.Description, entity.Description);
        Assert.Equal(request.Cost, entity.Cost);
    }

    [Fact]
    public async Task Handle_NameTakenByOtherEntity_ReturnsFailure()
    {
        var request = new UpdateMedicalProcedureRequest
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Description = "description",
            Cost = 123
        };

        _repository.GetAsync(Arg.Is<GetMedicalProcedureRequest>(r => r.NameExact == request.Name)).Returns([new MedicalProcedure()]);

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }
}

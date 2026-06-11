using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ClinicApp.Tests.Unit.Handler.DrugClassHandler;

public class UpdateDrugClassHandlerTests
{
    private readonly IDrugClassRepository _repository;
    private readonly IValidator<UpdateDrugClassRequest> _validator;
    private readonly UpdateDrugClassHandler _handler;

    public UpdateDrugClassHandlerTests()
    {
        _repository = Substitute.For<IDrugClassRepository>();
        _validator = Substitute.For<IValidator<UpdateDrugClassRequest>>();
        _handler = new UpdateDrugClassHandler(_repository, _validator);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesEntity()
    {
        var request = new UpdateDrugClassRequest
        {
            Id = Guid.NewGuid(),
            Name = "name"
        };

        var entity = new DrugClass();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        _repository.GetAsync(Arg.Is<GetDrugClassRequest>(r => r.Name == request.Name)).Returns([]);

        _validator.Validate(request).Returns(new ValidationResult());

        var response = await _handler.Handle(request);

        await _repository.Received(1).UpdateAsync(entity);

        Assert.True(response.IsSuccess);
        Assert.Equal(request.Name, entity.Name);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsFailure()
    {
        var request = new UpdateDrugClassRequest
        {
            Id = Guid.NewGuid(),
            Name = "name"
        };

        var entity = new DrugClass();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        _repository.GetAsync(Arg.Is<GetDrugClassRequest>(r => r.Name == request.Name)).Returns([]);

        _validator.Validate(request).Returns(new ValidationResult([new ValidationFailure("prop", "error")]));

        var response = await _handler.Handle(request);

        Assert.False(response.IsSuccess);
    }

    [Fact]
    public async Task Handle_EntityNotFound_ReturnsFailure()
    {
        var request = new UpdateDrugClassRequest
        {
            Id = Guid.NewGuid(),
            Name = "name"
        };

        var entity = new DrugClass();

        _repository.GetByIdAsync(request.Id).ReturnsNull();

        var response = await _handler.Handle(request);

        Assert.False(response.IsSuccess);
    }

    [Fact]
    public async Task Handle_NameTaken_ReturnsFailure()
    {
        var request = new UpdateDrugClassRequest
        {
            Id = Guid.NewGuid(),
            Name = "name"
        };

        var entity = new DrugClass();

        _repository.GetByIdAsync(request.Id).Returns(entity);

        _repository.GetAsync(Arg.Is<GetDrugClassRequest>(r => r.Name == request.Name)).Returns([new DrugClass()]);

        var response = await _handler.Handle(request);

        Assert.False(response.IsSuccess);
    }
}

using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;

namespace ClinicApp.Tests.Unit.Handler.DrugClassHandler;

public class AddDrugClassHandlerTests
{
    private readonly IDrugClassRepository _repository;
    private readonly AddDrugClassHandler _handler;
    private readonly IValidator<AddDrugClassRequest> _validator;

    public AddDrugClassHandlerTests()
    {
        _repository = Substitute.For<IDrugClassRepository>();
        _validator = Substitute.For<IValidator<AddDrugClassRequest>>();
        _handler = new AddDrugClassHandler(_repository, _validator);
    }

    [Fact]
    public async Task Handle_ValidRequest_AddsEntity()
    {
        var request = new AddDrugClassRequest
        {
            Name = "name",
        };

        _repository.GetAsync(Arg.Is<GetDrugClassRequest>(r => r.Name == request.Name)).Returns([]);

        _validator.Validate(request).Returns(new ValidationResult());

        var result = await _handler.Handle(request);

        await _repository.Received(1).AddAsync(Arg.Any<DrugClass>());

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_NameTaken_ReturnsFailure()
    {
        var request = new AddDrugClassRequest
        {
            Name = "name",
        };

        _repository.GetAsync(Arg.Is<GetDrugClassRequest>(r => r.Name == request.Name)).Returns([new DrugClass()]);

        _validator.Validate(request).Returns(new ValidationResult());

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsFailure()
    {
        var request = new AddDrugClassRequest
        {
            Name = "name",
        };

        _repository.GetAsync(Arg.Is<GetDrugClassRequest>(r => r.Name == request.Name)).Returns([]);

        _validator.Validate(request).Returns(new ValidationResult([new ValidationFailure("prop", "err")]));

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ValidationErrors);
    }
}

using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ClinicApp.Tests.Unit.Handler.DrugHandler;

public class AddDrugHandlerTests
{
    private readonly IDrugRepository _drugRepository;
    private readonly IDrugClassRepository _drugClassRepository;
    private readonly IValidator<AddDrugRequest> _validator;
    private readonly AddDrugHandler _handler;

    public AddDrugHandlerTests()
    {
        _drugRepository = Substitute.For<IDrugRepository>();
        _drugClassRepository = Substitute.For<IDrugClassRepository>();
        _validator = Substitute.For<IValidator<AddDrugRequest>>();
        _handler = new AddDrugHandler(_drugRepository, _drugClassRepository, _validator);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        var request = new AddDrugRequest
        {
            ClassId = Guid.NewGuid(),
            BrandName = "bname"
        };

        _drugRepository.GetAsync(Arg.Is<GetDrugRequest>(r => r.BrandNameExact == request.BrandName))
            .Returns([]);

        _drugClassRepository.GetByIdAsync(request.ClassId).Returns(new DrugClass());

        _validator.Validate(request).Returns(new ValidationResult());

        var result = await _handler.Handle(request);

        await _drugRepository.Received(1).AddAsync(Arg.Any<Drug>());

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_BrandNameTaken_ReturnsFailure()
    {
        var request = new AddDrugRequest
        {
            BrandName = "bname",
            ClassId = Guid.NewGuid()
        };

        _drugClassRepository.GetByIdAsync(request.ClassId).Returns(new DrugClass());

        _drugRepository.GetAsync(Arg.Is<GetDrugRequest>(r => r.BrandNameExact == request.BrandName))
            .Returns([new Drug()]);

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ClassNotFound_ReturnsFailure()
    {
        var request = new AddDrugRequest
        {
            BrandName = "bname",
            ClassId = Guid.NewGuid()
        };

        _drugClassRepository.GetByIdAsync(request.ClassId).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsFailure()
    {
        var request = new AddDrugRequest
        {
            ClassId = Guid.NewGuid(),
            BrandName = "bname"
        };

        _drugRepository.GetAsync(Arg.Is<GetDrugRequest>(r => r.BrandNameExact == request.BrandName))
            .Returns([]);


        _drugClassRepository.GetByIdAsync(request.ClassId).Returns(new DrugClass());

        _validator.Validate(request).Returns(new ValidationResult([new ValidationFailure("prop", "error")]));

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ValidationErrors);
    }
}

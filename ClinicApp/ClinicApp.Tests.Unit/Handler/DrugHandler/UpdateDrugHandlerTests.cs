using ClinicApp.Application.Handler;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Repository;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Handler.DrugHandler;

public class UpdateDrugHandlerTests
{
    private readonly IDrugRepository _drugRepository;
    private readonly IDrugClassRepository _drugClassRepository;
    private readonly IValidator<UpdateDrugRequest> _validator;
    private readonly UpdateDrugHandler _handler;

    public UpdateDrugHandlerTests()
    {
        _drugRepository = Substitute.For<IDrugRepository>();
        _drugClassRepository = Substitute.For<IDrugClassRepository>();
        _validator = Substitute.For<IValidator<UpdateDrugRequest>>();
        _handler = new UpdateDrugHandler(_drugRepository, _drugClassRepository, _validator);
    }

    [Fact]
    public async Task Handle_ValidRequest_BrandNameChanged_UpdatesEntity()
    {
        var request = new UpdateDrugRequest
        {
            BrandName = "bname",
            ClassId = Guid.NewGuid(),
            GenericName = "gname",
            Id = Guid.NewGuid(),
            Price = 123
        };

        var entity = new Drug
        {
            Id = request.Id
        };

        _drugRepository.GetByIdAsync(request.Id).Returns(entity);
        _drugRepository.GetAsync(Arg.Is<GetDrugRequest>(r => r.BrandNameExact == request.BrandName)).Returns([]);

        _drugClassRepository.GetByIdAsync(request.ClassId).Returns(new DrugClass());

        _validator.Validate(request).Returns(new ValidationResult());

        var result = await _handler.Handle(request);

        await _drugRepository.Received(1).UpdateAsync(entity);

        Assert.True(result.IsSuccess);
        Assert.Equal(request.BrandName, entity.BrandName);
        Assert.Equal(request.ClassId, entity.ClassId);
        Assert.Equal(request.GenericName, entity.GenericName);
        Assert.Equal(request.Price, entity.Price);
    }

    [Fact]
    public async Task Handle_ValidRequest_BrandNameNotChanged_UpdatesEntity()
    {
        var request = new UpdateDrugRequest
        {
            BrandName = "bname",
            ClassId = Guid.NewGuid(),
            GenericName = "gname",
            Id = Guid.NewGuid()
        };

        var entity = new Drug
        {
            Id = request.Id
        };

        _drugRepository.GetByIdAsync(request.Id).Returns(entity);
        _drugRepository.GetAsync(Arg.Is<GetDrugRequest>(r => r.BrandNameExact == request.BrandName)).Returns([entity]);

        _drugClassRepository.GetByIdAsync(request.ClassId).Returns(new DrugClass());

        _validator.Validate(request).Returns(new ValidationResult());

        var result = await _handler.Handle(request);

        await _drugRepository.Received(1).UpdateAsync(entity);

        Assert.True(result.IsSuccess);
        Assert.Equal(request.BrandName, entity.BrandName);
        Assert.Equal(request.ClassId, entity.ClassId);
        Assert.Equal(request.GenericName, entity.GenericName);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsFailure()
    {
        var request = new UpdateDrugRequest
        {
            BrandName = "bname",
            ClassId = Guid.NewGuid(),
            GenericName = "gname",
            Id = Guid.NewGuid()
        };

        var entity = new Drug
        {
            Id = request.Id
        };

        _drugRepository.GetByIdAsync(request.Id).Returns(entity);
        _drugRepository.GetAsync(Arg.Is<GetDrugRequest>(r => r.BrandNameExact == request.BrandName)).Returns([entity]);

        _drugClassRepository.GetByIdAsync(request.ClassId).Returns(new DrugClass());

        _validator.Validate(request).Returns(new ValidationResult([new ValidationFailure("prop", "error")]));

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.ValidationErrors);
    }

    [Fact]
    public async Task Handle_NameTaken_ReturnsFailure()
    {
        var request = new UpdateDrugRequest
        {
            BrandName = "bname",
            ClassId = Guid.NewGuid(),
            GenericName = "gname",
            Id = Guid.NewGuid()
        };

        var entity = new Drug
        {
            Id = request.Id
        };

        _drugRepository.GetByIdAsync(request.Id).Returns(entity);
        _drugRepository.GetAsync(Arg.Is<GetDrugRequest>(r => r.BrandNameExact == request.BrandName)).Returns([new Drug()]);

        _drugClassRepository.GetByIdAsync(request.ClassId).Returns(new DrugClass());

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ClassNotFound_ReturnsFailure()
    {
        var request = new UpdateDrugRequest
        {
            BrandName = "bname",
            ClassId = Guid.NewGuid(),
            GenericName = "gname",
            Id = Guid.NewGuid()
        };

        var entity = new Drug
        {
            Id = request.Id
        };

        _drugRepository.GetByIdAsync(request.Id).Returns(entity);
        _drugRepository.GetAsync(Arg.Is<GetDrugRequest>(r => r.BrandNameExact == request.BrandName)).Returns([new Drug()]);

        _drugClassRepository.GetByIdAsync(request.ClassId).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_EntityNotFound_ReturnsFailure()
    {
        var request = new UpdateDrugRequest
        {
            BrandName = "bname",
            ClassId = Guid.NewGuid(),
            GenericName = "gname",
            Id = Guid.NewGuid()
        };

        var entity = new Drug
        {
            Id = request.Id
        };

        _drugRepository.GetByIdAsync(request.Id).ReturnsNull();

        var result = await _handler.Handle(request);

        Assert.False(result.IsSuccess);
    }
}

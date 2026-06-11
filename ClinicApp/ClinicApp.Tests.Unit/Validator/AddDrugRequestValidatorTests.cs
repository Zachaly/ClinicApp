using ClinicApp.Application.Validation;
using ClinicApp.Domain.Request.Add;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Validator;

public class AddDrugRequestValidatorTests
{
    private readonly AddDrugRequestValidator _validator;

    public AddDrugRequestValidatorTests()
    {
        _validator = new AddDrugRequestValidator();
    }

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        var request = new AddDrugRequest
        {
            BrandName = "bname",
            GenericName = "gname"
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void InvalidBrandNameLength_FailsValidation(int len) 
    {
        var request = new AddDrugRequest
        {
            BrandName = new string('a', len),
            GenericName = "gname"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void InvalidGenericNameLength_FailsValidation(int len)
    {
        var request = new AddDrugRequest
        {
            BrandName = "bname",
            GenericName = new string('a', len)
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }
}

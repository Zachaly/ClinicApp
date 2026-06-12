using ClinicApp.Application.Validation;
using ClinicApp.Domain.Request.Add;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Validator;

public class AddDrugClassRequestValidatorTests
{
    private readonly AddDrugClassRequestValidator _validator;

    public AddDrugClassRequestValidatorTests()
    {
        _validator = new AddDrugClassRequestValidator();
    }

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        var request = new AddDrugClassRequest
        {
            Name = "name"
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void InvalidNameLength_FailsValidation(int len)
    {
        var request = new AddDrugClassRequest
        {
            Name = new string('a', len)
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }
}

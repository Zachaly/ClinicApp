using ClinicApp.Application.Validation;
using ClinicApp.Domain.Request.Update;

namespace ClinicApp.Tests.Unit.Validator;

public class UpdateDrugRequestValidatorTests
{
    private UpdateDrugRequestValidator _validator;

    public UpdateDrugRequestValidatorTests()
    {
        _validator = new UpdateDrugRequestValidator();
    }

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        var request = new UpdateDrugRequest
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
        var request = new UpdateDrugRequest
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
        var request = new UpdateDrugRequest
        {
            BrandName = "bname",
            GenericName = new string('a', len)
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }
}

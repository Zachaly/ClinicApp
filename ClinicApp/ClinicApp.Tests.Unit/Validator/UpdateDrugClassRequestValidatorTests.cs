using ClinicApp.Application.Validation;
using ClinicApp.Domain.Request.Update;

namespace ClinicApp.Tests.Unit.Validator;

public class UpdateDrugClassRequestValidatorTests
{
    private readonly UpdateDrugClassRequestValidator _validator;

    public UpdateDrugClassRequestValidatorTests()
    {
        _validator = new UpdateDrugClassRequestValidator();
    }

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        var request = new UpdateDrugClassRequest
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
        var request = new UpdateDrugClassRequest
        {
            Name = new string('a', len)
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }
}

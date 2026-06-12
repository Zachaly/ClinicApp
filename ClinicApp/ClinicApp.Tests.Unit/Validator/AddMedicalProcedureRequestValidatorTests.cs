using ClinicApp.Application.Validation;
using ClinicApp.Domain.Request.Add;

namespace ClinicApp.Tests.Unit.Validator;

public class AddMedicalProcedureRequestValidatorTests
{
    private readonly AddMedicalProcedureRequestValidator _validator;

    public AddMedicalProcedureRequestValidatorTests()
    {
        _validator = new AddMedicalProcedureRequestValidator();
    }

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        var request = new AddMedicalProcedureRequest
        {
            Cost = 123,
            Description = "desc",
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
        var request = new AddMedicalProcedureRequest
        {
            Cost = 123,
            Description = "desc",
            Name = new string('a', len)
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void InvalidDescriptionLength_FailsValidation(int len)
    {
        var request = new AddMedicalProcedureRequest
        {
            Cost = 123,
            Description = new string('a', len),
            Name = "name"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Fact]
    public void InvalidCost_FailsValidation()
    {
        var request = new AddMedicalProcedureRequest
        {
            Cost = -1,
            Description = "desc",
            Name = "name"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }
}

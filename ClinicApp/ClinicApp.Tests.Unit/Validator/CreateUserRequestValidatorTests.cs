using ClinicApp.Application.User.Validation;
using ClinicApp.Domain.Request;

namespace ClinicApp.Tests.Unit.Validator;

public class CreateUserRequestValidatorTests
{
    private readonly CreateUserRequestValidator _validator;

    public CreateUserRequestValidatorTests()
    {
        _validator = new CreateUserRequestValidator();
    }

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        var request = new CreateUserRequest
        {
            Email = "email@email.com",
            FirstName = "fname",
            LastName = "lname",
            Password = "pass",
            UserName = "user"
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void InvalidEmail_FailsValidation()
    {
        var request = new CreateUserRequest
        {
            Email = "email",
            FirstName = "fname",
            LastName = "lname",
            UserName = "user"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void InvalidFirstNameLength_FailsValidation(int len)
    {
        var request = new CreateUserRequest
        {
            Email = "email@email.com",
            FirstName = new string('a', len),
            LastName = "lname",
            UserName = "user"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void InvalidLastNameLength_FailsValidation(int len)
    {
        var request = new CreateUserRequest
        {
            Email = "email@email.com",
            FirstName = "fname",
            LastName = new string('a', len),
            UserName = "user"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void InvalidUserNameLength_FailsValidation(int len)
    {
        var request = new CreateUserRequest
        {
            Email = "email@email.com",
            FirstName = "fname",
            LastName = "lname",
            UserName = new string('a', len)
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }
}

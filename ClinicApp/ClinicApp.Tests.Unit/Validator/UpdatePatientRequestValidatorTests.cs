using ClinicApp.Application.Validation;
using ClinicApp.Domain.Request;

namespace ClinicApp.Tests.Unit.Validator;

public class UpdatePatientRequestValidatorTests
{
    private readonly UpdatePatientRequestValidator _validator;

    public UpdatePatientRequestValidatorTests()
    {
        _validator = new UpdatePatientRequestValidator();
    }

    [Fact]
    public void ValidRequest_ValidationSucceded()
    {
        var request = new UpdatePatientRequest
        {
            Address = "street 1/2",
            City = "krakow",
            FirstName = "fname",
            LastName = "lname",
            PostalCode = "12-345"
        };

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(76)]
    public void InvalidAddressLength_ValidationFails(int len)
    {
        var request = new UpdatePatientRequest
        {
            Address = new string('a', len),
            City = "krakow",
            FirstName = "fname",
            LastName = "lname",
            PostalCode = "12-345"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void InvalidFirstNameLength_ValidationFails(int len)
    {
        var request = new UpdatePatientRequest
        {
            Address = "street 1/2",
            City = "krakow",
            FirstName = new string('a', len),
            LastName = "lname",
            PostalCode = "12-345"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void InvalidLastNameLength_ValidationFails(int len)
    {
        var request = new UpdatePatientRequest
        {
            Address = "street 1/2",
            City = "krakow",
            FirstName = "fname",
            LastName = new string('a', len),
            PostalCode = "12-345"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void InvalidCityLength_ValidationFails(int len)
    {
        var request = new UpdatePatientRequest
        {
            Address = "street 1/2",
            City = new string('a', len),
            FirstName = "fname",
            LastName = "lname",
            PostalCode = "12-345"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("123-45")]
    [InlineData("1a-345")]
    [InlineData("12345")]
    public void InvalidPostalCode_ValidationFails(string postalCode)
    {
        var request = new UpdatePatientRequest
        {
            Address = "street 1/2",
            City = "city",
            FirstName = "fname",
            LastName = "lname",
            PostalCode = postalCode
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }
}

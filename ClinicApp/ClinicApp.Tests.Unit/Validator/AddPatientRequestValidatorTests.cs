using ClinicApp.Application.Validation;
using ClinicApp.Domain.Request.Add;

namespace ClinicApp.Tests.Unit.Validator;

public class AddPatientRequestValidatorTests
{
    private readonly AddPatientRequestValidator _validator;

    public AddPatientRequestValidatorTests()
    {
        _validator = new AddPatientRequestValidator();
    }

    [Fact]
    public void ValidRequest_ValidationSucceded()
    {
        var request = new AddPatientRequest
        {
            Address = "street 1/2",
            BirthDate = DateTime.Now.AddDays(-1),
            City = "krakow",
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = "12345678901",
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
        var request = new AddPatientRequest
        {
            Address = new string('a', len),
            BirthDate = DateTime.Now,
            City = "krakow",
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = "12345678901",
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
        var request = new AddPatientRequest
        {
            Address = "street 1/2",
            BirthDate = DateTime.Now,
            City = "krakow",
            FirstName = new string('a', len),
            LastName = "lname",
            PeselNumber = "12345678901",
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
        var request = new AddPatientRequest
        {
            Address = "street 1/2",
            BirthDate = DateTime.Now,
            City = "krakow",
            FirstName = "fname",
            LastName = new string('a', len),
            PeselNumber = "12345678901",
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
        var request = new AddPatientRequest
        {
            Address = "street 1/2",
            BirthDate = DateTime.Now,
            City = new string('a', len),
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = "12345678901",
            PostalCode = "12-345"
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("123a5678901")]
    [InlineData("1234567890121")]
    public void InvalidPeselNumber_ValidationFails(string pesel)
    {
        var request = new AddPatientRequest
        {
            Address = "street 1/2",
            BirthDate = DateTime.Now,
            City = "city",
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = pesel,
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
        var request = new AddPatientRequest
        {
            Address = "street 1/2",
            BirthDate = DateTime.Now,
            City = "city",
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = "12345678901",
            PostalCode = postalCode
        };

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
    }
}

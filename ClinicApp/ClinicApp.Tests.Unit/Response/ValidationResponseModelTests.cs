using ClinicApp.Domain.Response;

namespace ClinicApp.Tests.Unit.Response;

public class ValidationResponseModelTests
{
    [Fact]
    public void Constructor_NoArguments_ReturnsSuccess()
    {
        var response = new ValidationResponseModel();

        Assert.True(response.IsSuccess);
    }

    [Fact]
    public void Constructor_DictionaryArgument_ReturnsFailureAndSetsDefaltMessage()
    {
        var errors = new Dictionary<string, string[]>();

        var response = new ValidationResponseModel(errors);

        Assert.False(response.IsSuccess);
        Assert.NotNull(response.Error);
        Assert.Equal(errors, response.ValidationErrors);
    }

    [Fact]
    public void Constructor_DictionaryAndStringArguments_ReturnsFailure()
    {
        var message = "error";
        var errors = new Dictionary<string, string[]>();

        var response = new ValidationResponseModel(message, errors);

        Assert.False(response.IsSuccess);
        Assert.Equal(message, response.Error);
        Assert.Equal(errors, response.ValidationErrors);
    }

    [Fact]
    public void Constructor_StringArgument_ReturnsFailure()
    {
        var message = "error";

        var response = new ValidationResponseModel(message);

        Assert.False(response.IsSuccess);
        Assert.Equal(message, response.Error);
        Assert.Null(response.ValidationErrors);
    }
}

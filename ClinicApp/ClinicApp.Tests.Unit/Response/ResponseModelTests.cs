using ClinicApp.Domain.Response;

namespace ClinicApp.Tests.Unit.Response;

public class ResponseModelTests
{
    [Fact]
    public void Constructor_NoArguments_ReturnsSuccess()
    {
        var response = new ResponseModel();

        Assert.True(response.IsSuccess);
    }

    [Fact]
    public void Constructor_StringArgument_ReturnsFailure()
    {
        var error = "error";
        var response = new ResponseModel(error);

        Assert.Equal(error, response.Error);
        Assert.False(response.IsSuccess);
    }
}

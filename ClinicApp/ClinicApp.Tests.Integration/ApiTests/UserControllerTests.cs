using ClinicApp.Application.User.Model;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Infrastructure.Authorization;
using ClinicApp.Tests.Integration.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace ClinicApp.Tests.Integration.ApiTests;

[Collection(TestCollections.Collection1)]
public class UserControllerTests : ApiTest
{
    const string Endpoint = "api/user";
    public UserControllerTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task AllEndpoints_RequireProperAuthorization()
    {
        List<HttpResponseMessage> unauthorized = [
            await _httpClient.GetAsync(Endpoint),
            await _httpClient.GetAsync($"{Endpoint}/{Guid.NewGuid()}"),
            await _httpClient.GetAsync($"{Endpoint}/count"),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/admin", new CreateUserRequest()),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/doctor", new CreateUserRequest()),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", new CreateUserRequest()),
            await _httpClient.DeleteAsync($"{Endpoint}/{Guid.NewGuid()}"),
            ];

        await AuthorizeDoctorAsync();
        List<HttpResponseMessage> forbidden = [
            await _httpClient.GetAsync(Endpoint),
            await _httpClient.GetAsync($"{Endpoint}/count"),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/admin", new CreateUserRequest()),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/doctor", new CreateUserRequest()),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", new CreateUserRequest()),
            await _httpClient.DeleteAsync($"{Endpoint}/{Guid.NewGuid()}"),
            ];

        await AuthorizeReceptionistAsync();
        forbidden.AddRange([
            await _httpClient.GetAsync(Endpoint),
            await _httpClient.GetAsync($"{Endpoint}/count"),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/admin", new CreateUserRequest()),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/doctor", new CreateUserRequest()),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", new CreateUserRequest()),
            await _httpClient.DeleteAsync($"{Endpoint}/{Guid.NewGuid()}"),
            ]);

        Assert.All(forbidden, r => Assert.Equal(HttpStatusCode.Forbidden, r.StatusCode));
        Assert.All(unauthorized, r => Assert.Equal(HttpStatusCode.Unauthorized, r.StatusCode));
    }

    [Fact]
    public async Task Get_WithFilter_ReturnsProperListOfUsers()
    {
        await AuthorizeAdminAsync();
        var userRequests = FakeDataFactory.CreateUserRequests(20);

        foreach(var r in userRequests.Take(10))
        {
            r.LastName = "test_" + r.LastName;
        }

        foreach(var r in userRequests)
        {
            await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", r);
        }

        var response = await _httpClient.GetAsync($"{Endpoint}?LastName=test&PageSize=5");
        var content = await response.Content.ReadFromJsonAsync<List<UserModel>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(5, content.Count);
        Assert.All(content, u =>
        {
            Assert.StartsWith("test", u.LastName);
            Assert.Contains(u.Claims, c => c == AuthClaimNames.Receptionist);
        });
    }

    [Fact]
    public async Task Get_WithoutFilter_Returns10Results()
    {
        await AuthorizeAdminAsync();
        var userRequests = FakeDataFactory.CreateUserRequests(20);

        foreach (var r in userRequests)
        {
            await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", r);
        }

        var response = await _httpClient.GetAsync($"{Endpoint}");
        var content = await response.Content.ReadFromJsonAsync<List<UserModel>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(10, content.Count);
    }

    [Fact]
    public async Task GetById_ReturnsCorrectUser()
    {
        var userId = (await AuthorizeAdminAsync()).UserId;

        var user = _dbContext.Users.First(u => u.Id == userId);

        var response = await _httpClient.GetAsync($"{Endpoint}/{userId}");
        var content = await response.Content.ReadFromJsonAsync<UserModel>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(user.UserName, content.UserName);
        Assert.Equal(user.FirstName, content.FirstName);
        Assert.Equal(user.LastName, content.LastName);
        Assert.Equal(user.Email, content.Email);
        Assert.Contains(content.Claims, c => c == AuthClaimNames.Admin);
    }

    [Fact]
    public async Task GetById_UserNotFound_ReturnsNoContent()
    {
        await AuthorizeAdminAsync();

        var response = await _httpClient.GetAsync($"{Endpoint}/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCount_ReturnsProperCount()
    {
        var count = 30;
        await AuthorizeAdminAsync();
        var userRequests = FakeDataFactory.CreateUserRequests(count);

        foreach (var r in userRequests)
        {
            await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", r);
        }

        var response = await _httpClient.GetAsync($"{Endpoint}/count");
        var content = await response.Content.ReadFromJsonAsync<int>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //+1 because there also admin created on startup
        Assert.Equal(count + 1, content);
    }

    [Fact]
    public async Task AddAdmin_AddsAdminUser()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/admin", request);

        var createdUser = _dbContext.Users.FirstOrDefault(x => x.Email == request.Email);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdUser);
        Assert.Equal(request.FirstName, createdUser.FirstName);
        Assert.Equal(request.LastName, createdUser.LastName);
        Assert.Equal(request.UserName, createdUser.UserName);
        Assert.Contains(_dbContext.UserClaims, c => c.ClaimValue == AuthClaimNames.Admin && c.UserId == createdUser.Id);
    }

    [Fact]
    public async Task AddDoctor_AddsDoctorUser()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/doctor", request);

        var createdUser = _dbContext.Users.FirstOrDefault(x => x.Email == request.Email);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdUser);
        Assert.Equal(request.FirstName, createdUser.FirstName);
        Assert.Equal(request.LastName, createdUser.LastName);
        Assert.Equal(request.UserName, createdUser.UserName);
        Assert.Contains(_dbContext.UserClaims, c => c.ClaimValue == AuthClaimNames.Doctor && c.UserId == createdUser.Id);
    }

    [Fact]
    public async Task AddReceptionist_AddsAdminUser()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", request);

        var createdUser = _dbContext.Users.FirstOrDefault(x => x.Email == request.Email);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(createdUser);
        Assert.Equal(request.FirstName, createdUser.FirstName);
        Assert.Equal(request.LastName, createdUser.LastName);
        Assert.Equal(request.UserName, createdUser.UserName);
        Assert.Contains(_dbContext.UserClaims, c => c.ClaimValue == AuthClaimNames.Receptionist && c.UserId == createdUser.Id);
    }

    [Fact]
    public async Task AddAdmin_InvalidRequest_ReturnsBadRequestWithValidationErrors()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];
        request.FirstName = string.Empty;

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/admin", request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        var createdUser = _dbContext.Users.FirstOrDefault();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotEmpty(content.ValidationErrors["FirstName"]);
        Assert.DoesNotContain(_dbContext.Users, x => x.Email == request.Email);
    }

    [Fact]
    public async Task AddDoctor_InvalidRequest_ReturnsBadRequestWithValidationErrors()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];
        request.FirstName = string.Empty;

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/doctor", request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        var createdUser = _dbContext.Users.FirstOrDefault();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotEmpty(content.ValidationErrors["FirstName"]);
        Assert.DoesNotContain(_dbContext.Users, x => x.Email == request.Email);
    }

    [Fact]
    public async Task AddReceptionist_InvalidRequest_ReturnsBadRequestWithValidationErrors()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];
        request.FirstName = string.Empty;

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        var createdUser = _dbContext.Users.FirstOrDefault();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotEmpty(content.ValidationErrors["FirstName"]);
        Assert.DoesNotContain(_dbContext.Users, x => x.Email == request.Email);
    }

    [Fact]
    public async Task AddAdmin_EmailTaken_ReturnsBadRequest()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];
        request.Email = "admin@clinicapp.com";

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/admin", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddAdmin_UsernameTaken_ReturnsBadRequest()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];
        request.UserName = "admin";

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/admin", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddDoctor_EmailTaken_ReturnsBadRequest()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];
        request.Email = "admin@clinicapp.com";

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/doctor", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddDoctor_UsernameTaken_ReturnsBadRequest()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];
        request.UserName = "admin";

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/doctor", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReceptionist_EmailTaken_ReturnsBadRequest()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];
        request.Email = "admin@clinicapp.com";

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReceptionist_UsernameTaken_ReturnsBadRequest()
    {
        await AuthorizeAdminAsync();

        var request = FakeDataFactory.CreateUserRequests(1)[0];
        request.UserName = "admin";

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteUserById_DeletesSpecifiedUser()
    {
        await AuthorizeAdminAsync();

        var userRequest = FakeDataFactory.CreateUserRequests(1)[0];

        await _httpClient.PostAsJsonAsync($"{Endpoint}/receptionist", userRequest);

        var userId = _dbContext.Users.First(u => u.Email == userRequest.Email).Id;

        var response = await _httpClient.DeleteAsync($"{Endpoint}/{userId}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.DoesNotContain(_dbContext.Users, u => u.Id == userId);
    }

    [Fact]
    public async Task DeleteUserById_UserNotFound_ReturnsNoContent()
    {
        await AuthorizeAdminAsync();

        var response = await _httpClient.DeleteAsync($"{Endpoint}/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}

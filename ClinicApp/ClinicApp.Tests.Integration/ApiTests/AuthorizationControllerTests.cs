using ClinicApp.Application.Authorization.Handler;
using ClinicApp.Domain.Response;
using ClinicApp.Infrastructure.Authorization;
using ClinicApp.Tests.Integration.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace ClinicApp.Tests.Integration.ApiTests;

[Collection(TestCollections.Collection1)]
public class AuthorizationControllerTests : ApiTest, IClassFixture<DatabaseFixture>
{
    private const string Endpoint = "api/authorization";

    public AuthorizationControllerTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task AllEndpoints_RequireProperAuthorization()
    {
        List<HttpResponseMessage> unauthorizedResponses = [
            await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/admin", new AddUserClaimRequest(Guid.NewGuid())),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/doctor", new AddUserClaimRequest(Guid.NewGuid())),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/receptionist", new AddUserClaimRequest(Guid.NewGuid())),
            ];

        await AuthorizeDoctorAsync();

        List<HttpResponseMessage> forbiddenResponses = [
            await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/admin", new AddUserClaimRequest(Guid.NewGuid())),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/doctor", new AddUserClaimRequest(Guid.NewGuid())),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/receptionist", new AddUserClaimRequest(Guid.NewGuid())),
            ];

        await AuthorizeReceptionistAsync();

        forbiddenResponses.AddRange([
            await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/admin", new AddUserClaimRequest(Guid.NewGuid())),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/doctor", new AddUserClaimRequest(Guid.NewGuid())),
            await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/receptionist", new AddUserClaimRequest(Guid.NewGuid()))
            ]);

        Assert.All(unauthorizedResponses, r => Assert.Equal(HttpStatusCode.Unauthorized, r.StatusCode));
        Assert.All(forbiddenResponses, r => Assert.Equal(HttpStatusCode.Forbidden, r.StatusCode));
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsProperUserData()
    {
        var loginRequest = new LoginRequest("admin", "Passw0rd!");

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);

        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(content.AuthToken);
        Assert.Equal(_dbContext.ApplicationUsers.First(u => u.UserName == loginRequest.Login).Id, content.UserId);
        Assert.Equivalent(_dbContext.UserClaims.Where(c => c.UserId == content.UserId).Select(c => c.ClaimValue), content.Claims);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsBadRequest()
    {
        var request = new LoginRequest("notuser", "notpassword");

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", request);

        var content = await GetContentFromBadRequestAsync<LoginResponse>(response);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.False(content.IsSuccess);
        Assert.Equal(default, content.AuthToken);
        Assert.Equal(default, content.UserId);
        Assert.Equal(default, content.Claims);
        Assert.NotEmpty(content.Error);
    }

    [Fact]
    public async Task AddAdminClaim_UserExists_AddsClaim()
    {
        await AuthorizeAdminAsync();
        var userRequest = FakeDataFactory.CreateUserRequests(1)[0];

        await _httpClient.PostAsJsonAsync("api/user/doctor", userRequest);

        var userId = _dbContext.Users.First(u => u.Email == userRequest.Email).Id;

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/admin", new AddUserClaimRequest(userId));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Contains(_dbContext.UserClaims, c => c.UserId == userId && c.ClaimValue == AuthClaimNames.Admin);
    }

    [Fact]
    public async Task AddDoctorClaim_UserExists_AddsClaim()
    {
        await AuthorizeAdminAsync();
        var userRequest = FakeDataFactory.CreateUserRequests(1)[0];

        await _httpClient.PostAsJsonAsync("api/user/receptionist", userRequest);

        var userId = _dbContext.Users.First(u => u.Email == userRequest.Email).Id;

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/doctor", new AddUserClaimRequest(userId));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Contains(_dbContext.UserClaims, c => c.UserId == userId && c.ClaimValue == AuthClaimNames.Doctor);
    }

    [Fact]
    public async Task AddReceptionistClaim_UserExists_AddsClaim()
    {
        await AuthorizeAdminAsync();
        var userRequest = FakeDataFactory.CreateUserRequests(1)[0];

        await _httpClient.PostAsJsonAsync("api/user/doctor", userRequest);

        var userId = _dbContext.Users.First(u => u.Email == userRequest.Email).Id;

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/receptionist", new AddUserClaimRequest(userId));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Contains(_dbContext.UserClaims, c => c.UserId == userId && c.ClaimValue == AuthClaimNames.Receptionist);
    }

    [Fact]
    public async Task AddAdminClaim_UserDoesNotExists_ReturnsBadRequest()
    {
        await AuthorizeAdminAsync();

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/admin", new AddUserClaimRequest(Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddDoctorClaim_UserDoesNotExists_AddsClaim()
    {
        await AuthorizeAdminAsync();

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/doctor", new AddUserClaimRequest(Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddReceptionistClaim_UserDoesNotExists_AddsClaim()
    {
        await AuthorizeAdminAsync();

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/receptionist", new AddUserClaimRequest(Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddAdminClaim_UserHasClaim_ReturnsNoContent()
    {
        await AuthorizeAdminAsync();
        var userRequest = FakeDataFactory.CreateUserRequests(1)[0];

        await _httpClient.PostAsJsonAsync("api/user/admin", userRequest);

        var userId = _dbContext.Users.First(u => u.Email == userRequest.Email).Id;

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/admin", new AddUserClaimRequest(userId));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task AddDoctorClaim_UserHasClaim_ReturnsNoContent()
    {
        await AuthorizeAdminAsync();
        var userRequest = FakeDataFactory.CreateUserRequests(1)[0];

        await _httpClient.PostAsJsonAsync("api/user/doctor", userRequest);

        var userId = _dbContext.Users.First(u => u.Email == userRequest.Email).Id;

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/doctor", new AddUserClaimRequest(userId));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task AddReceptionistClaim_UserHasClaim_ReturnsNoContent()
    {
        await AuthorizeAdminAsync();
        var userRequest = FakeDataFactory.CreateUserRequests(1)[0];

        await _httpClient.PostAsJsonAsync("api/user/receptionist", userRequest);

        var userId = _dbContext.Users.First(u => u.Email == userRequest.Email).Id;

        var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/claim/receptionist", new AddUserClaimRequest(userId));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}

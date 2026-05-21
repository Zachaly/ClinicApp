using ClinicApp.Application.Authorization.Handler;
using ClinicApp.Application.User.Handler;
using ClinicApp.Database;
using ClinicApp.Domain.Response;
using ClinicApp.Tests.Integration.Fixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ClinicApp.Tests.Integration.ApiTests;

public abstract class ApiTest : IClassFixture<DatabaseFixture>, IDisposable
{
    protected readonly HttpClient _httpClient;
    protected readonly ApplicationDbContext _dbContext;

    protected ApiTest(DatabaseFixture fixture)
    {
        var webFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:SqlServer"] = fixture.ConnectionString,
                    });
                });
            });

        _httpClient = webFactory.CreateClient();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(fixture.ConnectionString).Options;

        _dbContext = new ApplicationDbContext(options);
    }

    protected async Task<T?> GetContentFromBadRequestAsync<T>(HttpResponseMessage response)
    {
        var stringContent = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(stringContent);
    }

    protected Task<LoginResponse> AuthorizeAdminAsync()
        => Authorize("admin", "Passw0rd!");

    protected async Task<LoginResponse> AuthorizeDoctorAsync()
    {
        await AuthorizeAdminAsync();

        var addRequest = new CreateDoctorUserRequest
        {
            Email = "doctor@email.com",
            FirstName = "fname",
            LastName = "lname",
            Password = "Passw0rd!",
            UserName = "doctor"
        };

        await _httpClient.PostAsJsonAsync("api/user/doctor", addRequest);

        return await Authorize(addRequest.UserName, addRequest.Password);
    }

    protected async Task<LoginResponse> AuthorizeReceptionistAsync()
    {
        await AuthorizeAdminAsync();

        var addRequest = new CreateDoctorUserRequest
        {
            Email = "receptionist@email.com",
            FirstName = "fname",
            LastName = "lname",
            Password = "Passw0rd!",
            UserName = "receptionist"
        };

        await _httpClient.PostAsJsonAsync("api/user/receptionist", addRequest);

        return await Authorize(addRequest.UserName, addRequest.Password);
    }

    private async Task<LoginResponse> Authorize(string username, string password)
    {
        var request = new LoginRequest(username, password);

        var response = await _httpClient.PostAsJsonAsync("api/authorization/login", request);
        var content = await response.Content.ReadFromJsonAsync<LoginResponse>()!;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content.AuthToken);

        return content;
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
    }
}

using ClinicApp.Application.Model;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Tests.Integration.Fixture;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace ClinicApp.Tests.Integration.ApiTests;

public class PatientControllerTests : ApiTest, IClassFixture<DatabaseFixture>
{
    const string Endpoint = "api/patient";
    public PatientControllerTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Get_ReturnsListOfPatients()
    {
        await _dbContext.AddRangeAsync(FakeDataFactory.CreatePatients(20));
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.GetAsync(Endpoint);
        var content = await response.Content.ReadFromJsonAsync<List<PatientModel>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(10, content.Count);
        Assert.All(content, m => _dbContext.Patients.Any(p => p.Id == m.Id));
    }

    [Fact]
    public async Task GetById_ReturnsSpecifiedPatient()
    {
        await _dbContext.AddRangeAsync(FakeDataFactory.CreatePatients(10));
        await _dbContext.SaveChangesAsync();

        var expected = _dbContext.Patients.First();

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.GetAsync($"{Endpoint}/{expected.Id}");
        var content = await response.Content.ReadFromJsonAsync<PatientModel>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected.Id, content.Id);
        Assert.Equal(expected.PeselNumber, content.PeselNumber);
        Assert.Equal(expected.FirstName, content.FirstName);
        Assert.Equal(expected.LastName, content.LastName);
        Assert.Equal(expected.City, content.City);
        Assert.Equal(expected.Address, content.Address);
        Assert.Equal(expected.BirthDate, content.BirthDate);
        Assert.Equal(expected.PostalCode, content.PostalCode);
    }

    [Fact]
    public async Task GetById_EntityNotFound_ReturnsNotFound()
    {
        await AuthorizeReceptionistAsync();
        var response = await _httpClient.GetAsync($"{Endpoint}/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCount_ReturnsCorrectCount()
    {
        var count = 20;

        await _dbContext.AddRangeAsync(FakeDataFactory.CreatePatients(count));
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.GetAsync($"{Endpoint}/count");
        var content = await response.Content.ReadFromJsonAsync<int>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(count, content);
    }

    [Fact]
    public async Task Post_AddsNewPatient()
    {

        var request = new AddPatientRequest
        {
            Address = "street 1/2",
            BirthDate = new DateTimeOffset(2004, 3, 30, 0, 0, 0, TimeSpan.Zero),
            City = "Krakow",
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = "12345678901",
            PostalCode = "12-345"
        };

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains(_dbContext.Patients, p => p.PeselNumber == request.PeselNumber &&
                p.LastName == request.LastName &&
                p.FirstName == request.FirstName &&
                p.Address == request.Address &&
                p.BirthDate == request.BirthDate &&
                p.City == request.City &&
                p.PostalCode == request.PostalCode);
    }

    [Fact]
    public async Task Post_InvalidRequest_ReturnsBadRequest()
    {
        await AuthorizeReceptionistAsync();

        var request = new AddPatientRequest
        {
            Address = "street 1/2",
            BirthDate = new DateTimeOffset(2004, 3, 30, 0, 0, 0, TimeSpan.Zero),
            City = "Krakow",
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = "",
            PostalCode = "12-345"
        };

        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(content.ValidationErrors.Keys, k => k == "PeselNumber");
    }

    [Fact]
    public async Task Post_PeselTaken_ReturnsBadRequest()
    {
        var patient = FakeDataFactory.CreatePatients(1)[0];

        await _dbContext.AddAsync(patient);
        await _dbContext.SaveChangesAsync();

        var request = new AddPatientRequest
        {
            Address = "street 1/2",
            BirthDate = new DateTimeOffset(2004, 3, 30, 0, 0, 0, TimeSpan.Zero),
            City = "Krakow",
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = patient.PeselNumber,
            PostalCode = "12-345"
        };

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_UpdatesPatient()
    {
        var patient = FakeDataFactory.CreatePatients(1)[0];
        await _dbContext.AddAsync(patient);
        await _dbContext.SaveChangesAsync();

        var request = new UpdatePatientRequest
        {
            Address = "new address",
            City = "better city",
            FirstName = "more fun name",
            LastName = "new lastname",
            PostalCode = "12-345",
            Id = patient.Id
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

        await _dbContext.Entry(patient).ReloadAsync();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(request.Address, patient.Address);
        Assert.Equal(request.City, patient.City);
        Assert.Equal(request.FirstName, patient.FirstName);
        Assert.Equal(request.LastName, patient.LastName);
        Assert.Equal(request.PostalCode, patient.PostalCode);
    }

    [Fact]
    public async Task Put_InvalidRequest_ReturnsBadRequest()
    {
        var patient = FakeDataFactory.CreatePatients(1)[0];
        await _dbContext.AddAsync(patient);
        await _dbContext.SaveChangesAsync();

        var request = new UpdatePatientRequest
        {
            Address = "new address",
            City = "better city",
            FirstName = "more fun name",
            LastName = "",
            PostalCode = "12-345",
            Id = patient.Id
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        await _dbContext.Entry(patient).ReloadAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(content.ValidationErrors.Keys, k => k == "LastName");
        Assert.NotEqual(request.Address, patient.Address);
        Assert.NotEqual(request.City, patient.City);
        Assert.NotEqual(request.FirstName, patient.FirstName);
        Assert.NotEqual(request.LastName, patient.LastName);
        Assert.NotEqual(request.PostalCode, patient.PostalCode);
    }

    [Fact]
    public async Task Put_EntityNotFound_ReturnsBadRequest()
    {
        var request = new UpdatePatientRequest
        {
            Address = "new address",
            City = "better city",
            FirstName = "more fun name",
            LastName = "",
            PostalCode = "12-345",
            Id = Guid.NewGuid()
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request); 

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_MarksEntityAsDeleted()
    {
        var patient = FakeDataFactory.CreatePatients(1)[0];

        await _dbContext.AddAsync(patient);
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.DeleteAsync($"{Endpoint}/{patient.Id}");

        await _dbContext.Entry(patient).ReloadAsync();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.DoesNotContain(_dbContext.Patients, p => p.Id == patient.Id);
        Assert.NotNull(patient.DeletedOn);
    }
}

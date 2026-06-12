using ClinicApp.Application.Handler;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using ClinicApp.Tests.Integration.Fixture;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Net.Http.Json;

namespace ClinicApp.Tests.Integration.ApiTests;

[Collection(TestCollections.Collection2)]
public class MedicalProcedureControllerTests : ApiTest
{
    private const string Endpoint = "api/medicalProcedure";

    public MedicalProcedureControllerTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Get_ReturnsListOfModels()
    {
        var entities = FakeDataFactory.CreateMedicalProcedures(10);

        await _dbContext.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync(Endpoint);
        var content = await response.Content.ReadFromJsonAsync<List<MedicalProcedureModel>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equivalent(entities.Select(x => x.Id), content.Select(x => x.Id));
    }

    [Fact]
    public async Task GetById_ReturnsSpecifiedEntity()
    {
        var entities = FakeDataFactory.CreateMedicalProcedures(10);

        await _dbContext.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();

        var expected = entities[0];

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync($"{Endpoint}/{expected.Id}");
        var content = await response.Content.ReadFromJsonAsync<MedicalProcedureModel>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected.Id, content.Id);
        Assert.Equal(expected.Cost, content.Cost);
        Assert.Equal(expected.Description, content.Description);
        Assert.Equal(expected.Name, content.Name);
    }

    [Fact]
    public async Task GetById_EntityNotFound_ReturnsNotFound()
    {
        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync($"{Endpoint}/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCount_ReturnsProperCount()
    {
        var count = 20;

        await _dbContext.AddRangeAsync(FakeDataFactory.CreateMedicalProcedures(count));
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync($"{Endpoint}/count");
        var content = await response.Content.ReadFromJsonAsync<int>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(count, content);
    }

    [Fact]
    public async Task Post_ValidRequest_AddsNewEntity()
    {
        var request = new AddMedicalProcedureRequest
        {
            Cost = 123,
            Description = "desc",
            Name = "name"
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains(_dbContext.MedicalProcedures, p => p.Name == request.Name
            && p.Cost == request.Cost
            && p.Description == request.Description);
    }

    [Fact]
    public async Task Post_NameTaken_ReturnsBadRequest()
    {
        var entity = FakeDataFactory.CreateMedicalProcedures(1)[0];

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var request = new AddMedicalProcedureRequest
        {
            Cost = 123,
            Description = "desc",
            Name = entity.Name
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.DoesNotContain(_dbContext.MedicalProcedures, p => p.Name == request.Name
            && p.Cost == request.Cost
            && p.Description == request.Description);
    }

    [Fact]
    public async Task Post_InvalidRequest_ReturnsBadRequest()
    {
        var request = new AddMedicalProcedureRequest
        {
            Cost = 123,
            Description = "desc",
            Name = ""
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(content.ValidationErrors.Keys, k => k == "Name");
    }

    [Fact]
    public async Task Put_ValidRequest_UpdatesEntity()
    {
        var entity = FakeDataFactory.CreateMedicalProcedures(1)[0];

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateMedicalProcedureRequest
        {
            Id = entity.Id,
            Cost = 123,
            Description = "new desc",
            Name = "new name"
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

        await _dbContext.Entry(entity).ReloadAsync();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(request.Cost, entity.Cost);
        Assert.Equal(request.Description, entity.Description);
        Assert.Equal(request.Name, entity.Name);
    }

    [Fact]
    public async Task Put_NameTakenByAnotherEntity_ReturnsBadRequest()
    {
        var entities = FakeDataFactory.CreateMedicalProcedures(2);

        await _dbContext.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateMedicalProcedureRequest
        {
            Id = entities[0].Id,
            Cost = 123,
            Description = "new desc",
            Name = entities[1].Name
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_InvalidRequest_ReturnsBadRequest()
    {
        var entity = FakeDataFactory.CreateMedicalProcedures(1)[0];

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateMedicalProcedureRequest
        {
            Id = entity.Id,
            Cost = 123,
            Description = "new desc",
            Name = ""
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(content.ValidationErrors.Keys, k => k == "Name");
    }

    [Fact]
    public async Task Put_EntityNotFound_ReturnsBadRequest()
    {
        var entity = FakeDataFactory.CreateMedicalProcedures(1)[0];

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateMedicalProcedureRequest
        {
            Id = entity.Id,
            Cost = 123,
            Description = "new desc",
            Name = ""
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(content.ValidationErrors.Keys, k => k == "Name");
    }

    [Fact]
    public async Task Delete_DeletesSpecifiedEntity()
    {
        var entity = FakeDataFactory.CreateMedicalProcedures(1)[0];

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.DeleteAsync($"{Endpoint}/{entity.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.DoesNotContain(_dbContext.MedicalProcedures, p => p.Id == entity.Id);
    }
}

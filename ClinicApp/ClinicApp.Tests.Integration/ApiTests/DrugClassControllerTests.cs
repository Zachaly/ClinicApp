using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using ClinicApp.Tests.Integration.Fixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace ClinicApp.Tests.Integration.ApiTests;

[Collection(TestCollections.Collection2)]
public class DrugClassControllerTests : ApiTest
{
    private const string Endpoint = "api/drugclass";

    public DrugClassControllerTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Get_ReturnsListOfDrugClasses()
    {
        var classes = FakeDataFactory.CreateDrugClasses(5);

        await _dbContext.AddRangeAsync(classes);
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync(Endpoint);

        var content = await response.Content.ReadFromJsonAsync<List<DrugClassModel>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equivalent(classes.Select(x => x.Id), content.Select(x => x.Id));
    }

    [Fact]
    public async Task GetById_ReturnsSpecifiedDrugClass()
    {
        var classes = FakeDataFactory.CreateDrugClasses(5);

        await _dbContext.AddRangeAsync(classes);
        await _dbContext.SaveChangesAsync();

        var expected = classes.Last();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync($"{Endpoint}/{expected.Id}");
        var content = await response.Content.ReadFromJsonAsync<DrugClassModel>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected.Id, content.Id);
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
    public async Task GetCount_ReturnsCorrectCount()
    {
        var count = 20;

        var classes = FakeDataFactory.CreateDrugClasses(count);

        await _dbContext.AddRangeAsync(classes);
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync($"{Endpoint}/count");
        var content = await response.Content.ReadFromJsonAsync<int>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(count, content);
    }

    [Fact]
    public async Task Post_AddsNewDrugClass()
    {
        var request = new AddDrugClassRequest
        {
            Name = "name"
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains(_dbContext.DrugClasses, c => c.Name == request.Name);
    }

    [Fact]
    public async Task Post_NameTaken_ReturnsBadRequest()
    {
        var request = new AddDrugClassRequest
        {
            Name = "name"
        };

        await _dbContext.AddAsync(new DrugClass
        {
            Name = request.Name
        });
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_InvalidRequest_ReturnsBadRequest()
    {
        var request = new AddDrugClassRequest
        {
            Name = ""
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(content.ValidationErrors.Keys, k => k == "Name");
    }

    [Fact]
    public async Task Put_UpdatesDrugClass()
    {
        var entity = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateDrugClassRequest
        {
            Id = entity.Id,
            Name = "new name"
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

        await _dbContext.Entry(entity).ReloadAsync();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(request.Name, entity.Name);
    }

    [Fact]
    public async Task Put_InvalidRequest_ReturnsBadRequest()
    {
        var entity = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateDrugClassRequest
        {
            Id = entity.Id,
            Name = ""
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        await _dbContext.Entry(entity).ReloadAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotEqual(request.Name, entity.Name);
        Assert.Contains(content.ValidationErrors.Keys, k => k == "Name");
    }

    [Fact]
    public async Task Put_EntityNotFound_ReturnsBadRequest()
    {
        var request = new UpdateDrugClassRequest
        {
            Id = Guid.NewGuid(),
            Name = "new name"
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_NameTaken_ReturnsBadRequest()
    {
        var classes = FakeDataFactory.CreateDrugClasses(2);

        var updatedEntity = classes[0];

        await _dbContext.AddAsync(updatedEntity);
        await _dbContext.AddAsync(classes[1]);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateDrugClassRequest
        {
            Id = updatedEntity.Id,
            Name = classes[1].Name,
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        await _dbContext.Entry(updatedEntity).ReloadAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotEqual(request.Name, updatedEntity.Name);
    }

    [Fact]
    public async Task DeleteById_DeletesSpecifiedEntity()
    {
        var entity = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.DeleteAsync($"{Endpoint}/{entity.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.DoesNotContain(_dbContext.DrugClasses, c => c.Id == entity.Id);
    }
}

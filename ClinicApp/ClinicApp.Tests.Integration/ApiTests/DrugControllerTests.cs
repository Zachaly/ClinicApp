using ClinicApp.Application.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using ClinicApp.Tests.Integration.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace ClinicApp.Tests.Integration.ApiTests;

[Collection(TestCollections.Collection1)]
public class DrugControllerTests : ApiTest
{
    private const string Endpoint = "api/drug";

    public DrugControllerTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Get_ReturnsListOfDrugs()
    {
        var classes = FakeDataFactory.CreateDrugClasses(2);

        await _dbContext.AddRangeAsync(classes);

        var drugs = classes.Select(x => x.Id).SelectMany(id => FakeDataFactory.CreateDrugs(id, 5)).ToList();

        await _dbContext.AddRangeAsync(drugs);
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync(Endpoint);
        var content = await response.Content.ReadFromJsonAsync<List<DrugModel>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equivalent(drugs.Select(x => x.Id), content.Select(x => x.Id));
    }

    [Fact]
    public async Task GetById_ReturnsSpecifiedEntity()
    {
        var drugClass = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(drugClass);

        var drug = FakeDataFactory.CreateDrugs(drugClass.Id, 1)[0];
        await _dbContext.AddAsync(drug);

        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync($"{Endpoint}/{drug.Id}");
        var content = await response.Content.ReadFromJsonAsync<DrugModel>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(drug.GenericName, content.GenericName);
        Assert.Equal(drug.Id, content.Id);
        Assert.Equal(drug.BrandName, content.BrandName);
        Assert.Equal(drugClass.Name, content.ClassName);
        Assert.Equal(drug.ClassId, content.ClassId);
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
        var classes = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(classes);

        var count = 20;
        var drugs = FakeDataFactory.CreateDrugs(classes.Id, count);

        await _dbContext.AddRangeAsync(drugs);
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.GetAsync($"{Endpoint}/count");
        var content = await response.Content.ReadFromJsonAsync<int>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(count, content);
    }

    [Fact]
    public async Task Post_AddsNewDrug()
    {
        var drugClass = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(drugClass);
        await _dbContext.SaveChangesAsync();

        var request = new AddDrugRequest
        {
            ClassId = drugClass.Id,
            BrandName = "bname",
            GenericName = "gname",
            Price = 20
        };

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains(_dbContext.Drugs, d => d.ClassId == request.ClassId 
            && d.GenericName == request.GenericName 
            && d.BrandName == request.BrandName
            && d.Price == request.Price);
    }

    [Fact]
    public async Task Post_InvalidRequest_ReturnsBadRequest()
    {
        var drugClass = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(drugClass);
        await _dbContext.SaveChangesAsync();

        var request = new AddDrugRequest
        {
            ClassId = drugClass.Id,
            BrandName = "",
            GenericName = "gname",
            Price = 123
        };

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains(content.ValidationErrors.Keys, k => k == "BrandName");
        Assert.DoesNotContain(_dbContext.Drugs, d => d.ClassId == request.ClassId
            && d.GenericName == request.GenericName
            && d.BrandName == request.BrandName
            && d.Price == request.Price);
    }

    [Fact]
    public async Task Post_ClassNotFound_ReturnsBadRequest()
    {
        var request = new AddDrugRequest
        {
            ClassId = Guid.NewGuid(),
            BrandName = "",
            GenericName = "gname",
            Price = 123
        };

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_BrandNameTaken_ReturnsBadRequest()
    {
        var drugClass = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(drugClass);

        var request = new AddDrugRequest
        {
            ClassId = drugClass.Id,
            BrandName = "bname",
            GenericName = "gname",
            Price = 123
        };

        await _dbContext.AddAsync(new Drug
        {
            BrandName = "bname",
            GenericName = "gname2",
            ClassId = drugClass.Id,
            Price = 123
        });
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();
        var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_UpdatesSpecifiedDrug()
    {
        var drugClasses = FakeDataFactory.CreateDrugClasses(2);

        await _dbContext.AddRangeAsync(drugClasses);

        var entity = new Drug
        {
            ClassId = drugClasses[0].Id,
            BrandName = "bname",
            GenericName = "gname",
            Price = 123
        };

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateDrugRequest
        {
            Id = entity.Id,
            BrandName = "new bname",
            GenericName = "new gname",
            ClassId = drugClasses[1].Id,
            Price = 321
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

        await _dbContext.Entry(entity).ReloadAsync();

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(entity.ClassId, request.ClassId);
        Assert.Equal(entity.GenericName, request.GenericName);
        Assert.Equal(entity.BrandName, request.BrandName);
        Assert.Equal(entity.Price, request.Price);
    }

    [Fact]
    public async Task Put_EntityNotFound_ReturnsBadRequest()
    {
        var drugClass = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(drugClass);

        var request = new UpdateDrugRequest
        {
            Id = Guid.NewGuid(),
            BrandName = "new bname",
            GenericName = "new gname",
            ClassId = drugClass.Id,
            Price = 321
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Put_BrandNameTaken_ReturnsBadRequest()
    {
        var drugClasses = FakeDataFactory.CreateDrugClasses(2);

        await _dbContext.AddRangeAsync(drugClasses);

        var entity = new Drug
        {
            ClassId = drugClasses[0].Id,
            BrandName = "bname",
            GenericName = "gname",
            Price = 123
        };

        await _dbContext.AddAsync(entity);

        var request = new UpdateDrugRequest
        {
            Id = entity.Id,
            BrandName = "new bname",
            GenericName = "new gname",
            ClassId = drugClasses[1].Id,
            Price = 321
        };

        await _dbContext.AddAsync(new Drug
        {
            BrandName = request.BrandName,
            GenericName = "gname",
            ClassId = drugClasses[0].Id
        });
        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

        await _dbContext.Entry(entity).ReloadAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotEqual(entity.ClassId, request.ClassId);
        Assert.NotEqual(entity.GenericName, request.GenericName);
        Assert.NotEqual(entity.BrandName, request.BrandName);
    }

    [Fact]
    public async Task Put_ClassNotFound_ReturnsBadRequest()
    {
        var drugClasses = FakeDataFactory.CreateDrugClasses(2);

        await _dbContext.AddRangeAsync(drugClasses);

        var entity = new Drug
        {
            ClassId = drugClasses[0].Id,
            BrandName = "bname",
            GenericName = "gname",
            Price = 123
        };

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateDrugRequest
        {
            Id = entity.Id,
            BrandName = "new bname",
            GenericName = "new gname",
            ClassId = Guid.NewGuid(),
            Price = 321
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);

        await _dbContext.Entry(entity).ReloadAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotEqual(entity.ClassId, request.ClassId);
        Assert.NotEqual(entity.GenericName, request.GenericName);
        Assert.NotEqual(entity.BrandName, request.BrandName);
        Assert.NotEqual(entity.Price, request.Price);
    }

    [Fact]
    public async Task Put_InvalidRequest_ReturnsBadRequest()
    {
        var drugClasses = FakeDataFactory.CreateDrugClasses(2);

        await _dbContext.AddRangeAsync(drugClasses);

        var entity = new Drug
        {
            ClassId = drugClasses[0].Id,
            BrandName = "bname",
            GenericName = "gname",
            Price = 123
        };

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        var request = new UpdateDrugRequest
        {
            Id = entity.Id,
            BrandName = "new bname",
            GenericName = "",
            ClassId = drugClasses[1].Id,
            Price = 321
        };

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.PutAsJsonAsync(Endpoint, request);
        var content = await GetContentFromBadRequestAsync<ValidationResponseModel>(response);
        await _dbContext.Entry(entity).ReloadAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotEqual(entity.ClassId, request.ClassId);
        Assert.NotEqual(entity.GenericName, request.GenericName);
        Assert.NotEqual(entity.BrandName, request.BrandName);
        Assert.NotEqual(entity.Price, request.Price);
        Assert.Contains(content.ValidationErrors.Keys, k => k == "GenericName");
    }

    [Fact]
    public async Task Delete_DeletesSpecifiedEntity()
    {
        var drugClass = FakeDataFactory.CreateDrugClasses(1)[0];

        await _dbContext.AddAsync(drugClass);

        var drug = FakeDataFactory.CreateDrugs(drugClass.Id, 1)[0];
        await _dbContext.AddAsync(drug);

        await _dbContext.SaveChangesAsync();

        await AuthorizeReceptionistAsync();

        var response = await _httpClient.DeleteAsync($"{Endpoint}/{drug.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.DoesNotContain(_dbContext.Drugs, d => d.Id == drug.Id);
    }
}

using ClinicApp.Application;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Add;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Mapper;

public class DrugClassModelMapperTests
{
    private readonly DrugClassModelMapper _mapper;

    public DrugClassModelMapperTests()
    {
        _mapper = new DrugClassModelMapper();
    }

    [Fact]
    public void MapRequestToEntity_CreatesProperEntity()
    {
        var request = new AddDrugClassRequest
        {
            Name = "name"
        };

        var entity = _mapper.MapRequestToEntity(request);

        Assert.Equal(request.Name, entity.Name);
    }

    [Fact]
    public void MapEntityToModel_CreatesProperModel()
    {
        var entity = new DrugClass
        {
            Name = "name",
            Id = Guid.NewGuid()
        };

        var model = _mapper.MapEntityToModel(entity);

        Assert.Equal(entity.Id, model.Id);
        Assert.Equal(entity.Name, model.Name);
    }
}

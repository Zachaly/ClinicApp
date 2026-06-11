using ClinicApp.Application;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Add;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicApp.Tests.Unit.Mapper;

public class DrugModelMapperTests
{
    private readonly DrugModelMapper _mapper;

    public DrugModelMapperTests()
    {
        _mapper = new DrugModelMapper();
    }

    [Fact]
    public void MapRequestToEntity_CreatesProperEntity()
    {
        var request = new AddDrugRequest
        {
            BrandName = "bname",
            ClassId = Guid.NewGuid(),
            GenericName = "gname"
        };

        var entity = _mapper.MapRequestToEntity(request);

        Assert.Equal(request.BrandName, entity.BrandName);
        Assert.Equal(request.GenericName, entity.GenericName);
        Assert.Equal(request.ClassId, entity.ClassId);
    }

    [Fact]
    public void MapEntityToModel_CreatesProperModel()
    {
        var entity = new Drug
        {
            Id = Guid.NewGuid(),
            BrandName = "bname",
            Class = new DrugClass
            {
                Name = "cname"
            },
            GenericName = "gname",
        };

        var model = _mapper.MapEntityToModel(entity);

        Assert.Equal(entity.Id, model.Id);
        Assert.Equal(entity.BrandName, model.BrandName);
        Assert.Equal(entity.Class.Name, model.ClassName);
        Assert.Equal(entity.GenericName, model.GenericName);
    }
}

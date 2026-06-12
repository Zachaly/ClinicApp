using ClinicApp.Application;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request.Add;

namespace ClinicApp.Tests.Unit.Mapper;

public class MedicalProcedureModelMapperTests
{
    private readonly MedicalProcedureModelMapper _mapper;

    public MedicalProcedureModelMapperTests()
    {
        _mapper = new MedicalProcedureModelMapper();
    }

    [Fact]
    public void MapEntityToModel_CreatesProperModel()
    {
        var entity = new MedicalProcedure
        {
            Cost = 123,
            Description = "desc",
            Id = Guid.NewGuid(),
            Name = "name"
        };

        var model = _mapper.MapEntityToModel(entity);

        Assert.Equal(entity.Id, model.Id);
        Assert.Equal(entity.Description, model.Description);
        Assert.Equal(entity.Name, model.Name);
        Assert.Equal(entity.Cost, model.Cost);
    }

    [Fact]
    public void MapRequestToEntity_CreatesProperEntity() 
    {
        var request = new AddMedicalProcedureRequest
        {
            Cost = 123,
            Description = "desc",
            Name = "name"
        };

        var entity = _mapper.MapRequestToEntity(request);

        Assert.Equal(request.Name, entity.Name);
        Assert.Equal(request.Description, entity.Description);
        Assert.Equal(request.Cost, entity.Cost);
    }
}

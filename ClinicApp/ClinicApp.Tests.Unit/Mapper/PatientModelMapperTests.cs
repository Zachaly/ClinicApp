using ClinicApp.Application;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;

namespace ClinicApp.Tests.Unit.Mapper;

public class PatientModelMapperTests
{
    private readonly PatientModelMapper _mapper;

    public PatientModelMapperTests()
    {
        _mapper = new PatientModelMapper();
    }

    [Fact]
    public void MapPatientToModel_ReturnsValidModel()
    {
        var entity = new Patient
        {
            Id = Guid.NewGuid(),
            Address = "addr",
            BirthDate = new DateTimeOffset(2004, 03, 3, 0, 0, 0, TimeSpan.Zero),
            City = "krakow",
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = "123",
            PostalCode = "12-123"
        };

        var model = _mapper.MapPatientToModel(entity);

        Assert.Equal(model.Id, entity.Id);
        Assert.Equal(model.Address, entity.Address);
        Assert.Equal(model.BirthDate, entity.BirthDate);
        Assert.Equal(model.LastName, entity.LastName);
        Assert.Equal(model.PeselNumber, entity.PeselNumber);
        Assert.Equal(model.PostalCode, entity.PostalCode);
        Assert.Equal(model.City, entity.City);
        Assert.Equal(model.FirstName, entity.FirstName);
    }

    [Fact]
    public void MapRequestToEntity_ReturnsCorrectEntity()
    {
        var request = new AddPatientRequest
        {
            Address = "addr",
            BirthDate = new DateTimeOffset(2004, 03, 3, 0, 0, 0, TimeSpan.Zero),
            City = "city",
            FirstName = "fname",
            LastName = "lname",
            PeselNumber = "1234",
            PostalCode = "12-123"
        };

        var entity = _mapper.MapRequestToEntity(request);

        Assert.Equal(entity.FirstName, request.FirstName);
        Assert.Equal(entity.LastName, request.LastName);
        Assert.Equal(entity.Address, request.Address);
        Assert.Equal(entity.BirthDate, request.BirthDate);
        Assert.Equal(entity.City, request.City);
        Assert.Equal(entity.PostalCode, request.PostalCode);
        Assert.Equal(entity.PeselNumber, request.PeselNumber);
        Assert.Null(entity.DeletedOn);
    }
}

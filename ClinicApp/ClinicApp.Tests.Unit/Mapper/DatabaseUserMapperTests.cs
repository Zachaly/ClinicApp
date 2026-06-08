using ClinicApp.Database.Model;
using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
using ClinicApp.Infrastructure.Mapper;

namespace ClinicApp.Tests.Unit.Mapper;

public class DatabaseUserMapperTests
{
    private readonly DatabaseUserMapper _mapper;

    public DatabaseUserMapperTests()
    {
        _mapper = new DatabaseUserMapper();
    }

    [Fact]
    public void MapRequestToDatabaseModel_CreatesProperEntity()
    {
        var request = new CreateUserRequest
        {
            Email = "email",
            FirstName = "fname",
            LastName = "lname",
            Password = "pass",
            UserName = "uname"
        };

        var user = _mapper.MapRequestToDatabaseModel(request);

        Assert.Equal(request.Email, user.Email);
        Assert.Equal(request.FirstName, user.FirstName);
        Assert.Equal(request.LastName, user.LastName);
        Assert.Equal(request.UserName, user.UserName);
    }

    [Fact]
    public void MapDatabaseModelToUser_CreatesProperEntity()
    {
        var dbUser = new DatabaseUser
        {
            Id = Guid.NewGuid(),
            FirstName = "fname",
            LastName = "lname",
            Email = "email",
            UserName = "uname",
        };

        var appUser = _mapper.MapDatabaseModelToUser(dbUser);

        Assert.Equal(dbUser.Id, appUser.Id);
        Assert.Equal(dbUser.FirstName, appUser.FirstName);
        Assert.Equal(dbUser.LastName, appUser.LastName);
        Assert.Equal(dbUser.Email, appUser.Email);
        Assert.Equal(dbUser.UserName, appUser.UserName);
    }

    [Fact]
    public void MapApplicationUserToDatabaseModel_ReturnsCorrectEntity()
    {
        var applicationUser = new ApplicationUser
        {
            Email = "email",
            FirstName = "fname",
            LastName = "lname",
            UserName = "uname",
            Id = Guid.NewGuid()
        };

        var dbUser = _mapper.MapApplicationUserToDatabaseModel(applicationUser);

        Assert.Equal(applicationUser.Id, dbUser.Id);
        Assert.Equal(applicationUser.FirstName, dbUser.FirstName);
        Assert.Equal(applicationUser.LastName, dbUser.LastName);
        Assert.Equal(applicationUser.UserName, dbUser.UserName);
        Assert.Equal(applicationUser.Email, dbUser.Email);
    }
}

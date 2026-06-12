using ClinicApp.Application.User;
using ClinicApp.Domain.Entity;

namespace ClinicApp.Tests.Unit.Mapper;

public class UserModelMapperTests
{
    private readonly UserModelMapper _mapper;

    public UserModelMapperTests()
    {
        _mapper = new UserModelMapper();
    }

    [Fact]
    public void MapEntityToModel_ReturnsCorrectModel()
    {
        var applicationUser = new ApplicationUser
        {
            Claims = [new UserClaim() { ClaimValue = "claim1" }, new UserClaim { ClaimValue = "claim2" }],
            Email = "email",
            FirstName = "fname",
            LastName = "lname",
            UserName = "uname",
            Id = Guid.NewGuid(),
        };

        var model = _mapper.MapEntityToModel(applicationUser);

        Assert.Equal(applicationUser.Id, model.Id);
        Assert.Equal(applicationUser.FirstName, model.FirstName);
        Assert.Equal(applicationUser.LastName, model.LastName);
        Assert.Equal(applicationUser.Email, model.Email);
        Assert.Equal(applicationUser.UserName, model.UserName);
        Assert.Equivalent(applicationUser.Claims.Select(x => x.ClaimValue), model.Claims);
    }
}

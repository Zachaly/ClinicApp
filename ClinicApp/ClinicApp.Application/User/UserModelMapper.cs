using ClinicApp.Application.User.Model;
using ClinicApp.Domain.Entity;
using Riok.Mapperly.Abstractions;

namespace ClinicApp.Application.User;

[Mapper]
public partial class UserModelMapper
{
    [MapProperty(nameof(ApplicationUser.Claims), nameof(UserModel.Claims), Use = nameof(MapClaims))]
    public partial UserModel MapApplicationUserToModel(ApplicationUser user);

    [UserMapping]
    private List<string> MapClaims(List<UserClaim> claims)
        => claims.Select(c => c.ClaimValue).ToList();
}

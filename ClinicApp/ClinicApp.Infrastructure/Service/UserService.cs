using ClinicApp.Database.Model;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;
using ClinicApp.Infrastructure.Authorization;
using ClinicApp.Infrastructure.Mapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using ClinicApp.Infrastructure.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ClinicApp.Infrastructure.Service;

public class UserService : IUserService
{
    private readonly UserManager<DatabaseUser> _userManager;
    private readonly DatabaseUserMapper _userMapper;
    private readonly AuthConfig _authConfig;

    public UserService(UserManager<DatabaseUser> userManager, IOptions<AuthConfig> authConfig)
    {
        _userManager = userManager;
        _userMapper = new DatabaseUserMapper();
        _authConfig = authConfig.Value;
    }

    public async Task<Guid> CheckPasswordAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if(user is null)
        {
            return Guid.Empty;
        }

        return await _userManager.CheckPasswordAsync(user, password) ? user.Id : Guid.Empty;
    }

    public Task<ValidationResponseModel> CreateAdminUserAsync(CreateUserRequest request)
        => AddUser(request, AuthClaimNames.Admin);

    public Task<ValidationResponseModel> CreateDoctorUserAsync(CreateUserRequest request)
        => AddUser(request, AuthClaimNames.Doctor);

    public Task<ValidationResponseModel> CreateReceptionistUserAsync(CreateUserRequest request)
        => AddUser(request, AuthClaimNames.Receptionist);

    public async Task<List<Claim>> GetClaimsAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return [];
        }

        return (await _userManager.GetClaimsAsync(user)).ToList();
    }

    public async Task<string> GenerateTokenAsync(Guid userId, List<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig.SecretKey));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _authConfig.Issuer,
            Audience = _authConfig.Audience,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(_authConfig.TokenLifetimeMinutes),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private Dictionary<string, string[]> MapIdentityErrors(IdentityResult identityResult)
    {
        var dict = new Dictionary<string, List<string>>();


        foreach (var e in identityResult.Errors)
        {
            if (dict.ContainsKey(e.Code))
            {
                dict[e.Code].Add(e.Description);
            }
            else
            {
                dict.Add(e.Code, [e.Description]);
            }
        }

        return dict.Select(e => new KeyValuePair<string, string[]>(e.Key, e.Value.ToArray())).ToDictionary();
    }

    private async Task<ValidationResponseModel> AddUser(CreateUserRequest request, string role)
    {
        var user = _userMapper.MapRequestToDatabaseModel(request);

        var userResult = await _userManager.CreateAsync(user, request.Password);

        if (!userResult.Succeeded)
        {
            return new ValidationResponseModel(MapIdentityErrors(userResult));
        }

        var claimResult = await _userManager.AddClaimAsync(user, new Claim(AuthClaimNames.RoleClaim, role));

        if (!claimResult.Succeeded)
        {
            return new ValidationResponseModel(MapIdentityErrors(claimResult));
        }

        return new ValidationResponseModel();
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if(user is null)
        {
            return;
        }

        await _userManager.DeleteAsync(user);
    }

    public Task<ResponseModel> AddAdminClaim(Guid userId)
        => AddClaim(userId, AuthClaimNames.Admin);

    public Task<ResponseModel> AddDoctorClaim(Guid userId)
        => AddClaim(userId, AuthClaimNames.Doctor);

    public Task<ResponseModel> AddReceptionistClaim(Guid userId)
        => AddClaim(userId, AuthClaimNames.Receptionist);

    private async Task<ResponseModel> AddClaim(Guid userId, string claim)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if(user is null)
        {
            return new ResponseModel("User not found");
        }

        var claims = await _userManager.GetClaimsAsync(user);

        if(claims.Any(c => c.Value == claim && c.Type == AuthClaimNames.RoleClaim))
        {
            return new ResponseModel();
        }

        var response = await _userManager.AddClaimAsync(user, new Claim(AuthClaimNames.RoleClaim, claim));

        if(!response.Succeeded)
        {
            return new ResponseModel("Failure while adding claim");
        }

        return new ResponseModel();
    }
}

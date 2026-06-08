using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using System.Security.Claims;

namespace ClinicApp.Domain.Service;

public interface IUserService
{
    Task<Guid> CheckPasswordAsync(string username, string password);
    Task<string> GenerateTokenAsync(Guid userId, List<Claim> claims);
    Task<List<Claim>> GetClaimsAsync(Guid userId);
    Task<ValidationResponseModel> CreateAdminUserAsync(CreateUserRequest request);
    Task<ValidationResponseModel> CreateDoctorUserAsync(CreateUserRequest request);
    Task<ValidationResponseModel> CreateReceptionistUserAsync(CreateUserRequest request);
    Task DeleteUserAsync(Guid userId);
    Task<ResponseModel> AddAdminClaim(Guid userId);
    Task<ResponseModel> AddDoctorClaim(Guid userId);
    Task<ResponseModel> AddReceptionistClaim(Guid userId);
}

using ClinicApp.Domain.Entity;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;

namespace ClinicApp.Domain.Service;

public interface IUserService
{
    Task<Guid> CheckPasswordAsync(string username, string password);
    Task<string> GenerateTokenAsync(Guid userId);
    Task<ValidationResponseModel> CreateAdminUserAsync(CreateUserRequest request);
    Task<ValidationResponseModel> CreateDoctorUserAsync(CreateUserRequest request);
    Task<ValidationResponseModel> CreateReceptionistUserAsync(CreateUserRequest request);
}

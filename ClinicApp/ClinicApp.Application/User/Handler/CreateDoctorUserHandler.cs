using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;
using FluentValidation;

namespace ClinicApp.Application.User.Handler;

public record CreateDoctorUserRequest : CreateUserRequest;

public class CreateDoctorUserHandler : CreateUserHandler<CreateDoctorUserRequest>
{
    public CreateDoctorUserHandler(IUserService userService, IValidator<CreateUserRequest> validator) : base(userService, validator)
    {
    }

    protected override Task<ValidationResponseModel> CreateUserAsync(CreateDoctorUserRequest request)
        => _userService.CreateDoctorUserAsync(request);
}

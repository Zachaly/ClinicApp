using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;
using FluentValidation;

namespace ClinicApp.Application.User.Handler;

public record CreateAdminUserRequest : CreateUserRequest;

public class CreateAdminUserHandler : CreateUserHandler<CreateAdminUserRequest>
{
    public CreateAdminUserHandler(IUserService userService, IValidator<CreateUserRequest> validator) : base(userService, validator)
    {
    }

    protected override Task<ValidationResponseModel> CreateUserAsync(CreateAdminUserRequest request)
        => _userService.CreateAdminUserAsync(request);
}

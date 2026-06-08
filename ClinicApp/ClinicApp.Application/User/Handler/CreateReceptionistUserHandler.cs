using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;
using FluentValidation;

namespace ClinicApp.Application.User.Handler;

public record CreateReceptionistUserRequest : CreateUserRequest;

public class CreateReceptionistUserHandler : CreateUserHandler<CreateReceptionistUserRequest>
{
    public CreateReceptionistUserHandler(IUserService userService, IValidator<CreateUserRequest> validator) : base(userService, validator)
    {
    }

    protected override Task<ValidationResponseModel> CreateUserAsync(CreateReceptionistUserRequest request)
        => _userService.CreateReceptionistUserAsync(request);
}

using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;
using FluentValidation;

namespace ClinicApp.Application.User.Handler;

public abstract class CreateUserHandler<TRequest> where TRequest : CreateUserRequest
{
    protected readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _validator;

    protected CreateUserHandler(IUserService userService, IValidator<CreateUserRequest> validator)
    {
        _userService = userService;
        _validator = validator;
    }

    public async Task<ValidationResponseModel> Handle(TRequest request) 
    {
        var validation = _validator.Validate(request);

        if(!validation.IsValid)
        {
            return new ValidationResponseModel(validation.ToDictionary());
        }

        return await CreateUserAsync(request);
    }

    protected abstract Task<ValidationResponseModel> CreateUserAsync(TRequest request);
}

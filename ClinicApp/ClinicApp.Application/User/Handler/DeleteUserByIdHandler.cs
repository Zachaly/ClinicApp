using ClinicApp.Domain.Response;
using ClinicApp.Domain.Service;

namespace ClinicApp.Application.User.Handler;

public record DeleteUserByIdRequest(Guid Id);

public class DeleteUserByIdHandler
{
    private readonly IUserService _service;

    public DeleteUserByIdHandler(IUserService userService)
    {
        _service = userService;
    }

    public async Task<ResponseModel> Handle(DeleteUserByIdRequest request)
    {
        await _service.DeleteUserAsync(request.Id);

        return new ResponseModel();
    }
}

using ClinicApp.Application.User.Handler;
using ClinicApp.Application.User.Model;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Infrastructure.Authorization;
using ClinicApp.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace ClinicApp.WebApi.Controllers;

[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public UserController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpGet]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<List<UserModel>>> Get([FromQuery] GetUserRequest request)
    {
        var response = await _messageBus.InvokeAsync<List<UserModel>>(request);

        return response.ReturnOkOrNotFound();
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserModel>> GetById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<UserModel?>(new GetUserByIdRequest(id));

        return response.ReturnOkOrNotFound();
    }

    [HttpGet("count")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<int>> GetCount([FromQuery] GetUserCountRequest request)
    {
        var response = await _messageBus.InvokeAsync<int>(request);

        return response.ReturnOkOrNotFound();
    }

    [HttpPost("admin")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<ValidationResponseModel>> PostAdmin(CreateAdminUserRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnCreatedOrBadRequest("user/");
    }


    [HttpPost("doctor")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<ValidationResponseModel>> PostDoctor(CreateDoctorUserRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnCreatedOrBadRequest("user/");
    }

    [HttpPost("receptionist")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<ValidationResponseModel>> PostReceptionist(CreateReceptionistUserRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnCreatedOrBadRequest("user/");
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<ResponseModel>> DeleteById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<ResponseModel>(new DeleteUserByIdRequest(id));

        return response.ReturnNoContentOrBadRequest();
    }
}

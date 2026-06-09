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
        var result = await _messageBus.InvokeAsync<List<UserModel>>(request);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserModel>> GetById(Guid id)
    {
        var result = await _messageBus.InvokeAsync<UserModel?>(new GetUserByIdRequest(id));

        return ResponseModelExtensions.ReturnOkOrNotFound(result);
    }

    [HttpGet("count")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<int>> GetCount([FromQuery] GetUserCountRequest request)
    {
        var result = await _messageBus.InvokeAsync<int>(request);

        return Ok(result);
    }

    [HttpPost("admin")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<ValidationResponseModel>> PostAdmin(CreateAdminUserRequest request)
    {
        var result = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return result.ReturnCreatedOrBadRequest("user/");
    }


    [HttpPost("doctor")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<ValidationResponseModel>> PostDoctor(CreateDoctorUserRequest request)
    {
        var result = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return result.ReturnCreatedOrBadRequest("user/");
    }

    [HttpPost("receptionist")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<ValidationResponseModel>> PostReceptionist(CreateReceptionistUserRequest request)
    {
        var result = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return result.ReturnCreatedOrBadRequest("user/");
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthPolicyNames.RequireAdmin)]
    public async Task<ActionResult<ResponseModel>> DeleteById(Guid id)
    {
        var result = await _messageBus.InvokeAsync<ResponseModel>(new DeleteUserByIdRequest(id));

        return result.ReturnNoContentOrBadRequest();
    }
}

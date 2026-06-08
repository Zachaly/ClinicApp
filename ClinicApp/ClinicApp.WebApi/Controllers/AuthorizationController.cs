using ClinicApp.Application.Authorization.Handler;
using ClinicApp.Domain.Response;
using ClinicApp.Infrastructure.Authorization;
using ClinicApp.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace ClinicApp.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = AuthPolicyNames.RequireAdmin)]
public class AuthorizationController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public AuthorizationController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await _messageBus.InvokeAsync<LoginResponse>(request);

        return result.ReturnOkOrBadRequest();
    }

    [HttpPost("claim/admin")]
    public async Task<ActionResult<ResponseModel>> AddAdminClaim(AddAdminClaimRequest request)
    {
        var result = await _messageBus.InvokeAsync<ResponseModel>(request);

        return result.ReturnNoContentOrBadRequest();
    }

    [HttpPost("claim/doctor")]
    public async Task<ActionResult<ResponseModel>> AddDoctorClaim(AddDoctorClaimRequest request)
    {
        var result = await _messageBus.InvokeAsync<ResponseModel>(request);

        return result.ReturnNoContentOrBadRequest();
    }

    [HttpPost("claim/receptionist")]
    public async Task<ActionResult<ResponseModel>> AddReceptionistClaim(AddReceptionistClaimRequest request)
    {
        var result = await _messageBus.InvokeAsync<ResponseModel>(request);

        return result.ReturnNoContentOrBadRequest();
    }
}

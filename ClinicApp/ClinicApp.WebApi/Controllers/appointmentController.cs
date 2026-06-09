using ClinicApp.Application.Model;
using ClinicApp.Domain.Request;
using ClinicApp.Domain.Response;
using ClinicApp.Infrastructure.Authorization;
using ClinicApp.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace ClinicApp.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public AppointmentController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpGet]
    public async Task<ActionResult<List<AppointmentModel>>> Get([FromQuery] GetAppointmentRequest request)
    {
        var response = await _messageBus.InvokeAsync<List<AppointmentModel>>(request);

        return ResponseModelExtensions.ReturnOkOrNotFound(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentModel>> GetById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<AppointmentModel?>(new GetAppointmentByIdRequest(id));

        return ResponseModelExtensions.ReturnOkOrNotFound(response);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount([FromQuery] GetAppointmentCountRequest request)
    {
        var response = await _messageBus.InvokeAsync<int>(request);

        return ResponseModelExtensions.ReturnOkOrNotFound(response);
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ValidationResponseModel>> Post(AddAppointmentRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnCreatedOrBadRequest("appointment");
    }

    [HttpPut]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ValidationResponseModel>> Put(UpdateAppointmentRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnNoContentOrBadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ResponseModel>> DeleteById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<ResponseModel>(new DeleteAppointmentByIdRequest(id));

        return response.ReturnNoContentOrBadRequest();
    }
}

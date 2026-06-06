using ClinicApp.Application.Handler;
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
public class PatientController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public PatientController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpGet]
    public async Task<ActionResult<List<PatientModel>>> Get([FromQuery] GetPatientRequest request)
    {
        var response = await _messageBus.InvokeAsync<List<PatientModel>>(request);

        return ResponseModelExtensions.ReturnOkOrNotFound(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientModel>> GetById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<PatientModel?>(new GetPatientByIdRequest(id));

        return ResponseModelExtensions.ReturnOkOrNotFound(response);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount([FromQuery] GetPatientCountRequest request)
    {
        var response = await _messageBus.InvokeAsync<int>(request);

        return ResponseModelExtensions.ReturnOkOrNotFound(response);
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ValidationResponseModel>> Post(AddPatientRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnCreatedOrBadRequest("patient");
    }

    [HttpPut]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ValidationResponseModel>> Put(UpdatePatientRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnNoContentOrBadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ResponseModel>> DeleteById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<ResponseModel>(new DeletePatientByIdRequest(id));

        return response.ReturnNoContentOrBadRequest();
    }
}

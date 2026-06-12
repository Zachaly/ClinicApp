using ClinicApp.Application.Handler;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using ClinicApp.Infrastructure.Authorization;
using ClinicApp.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace ClinicApp.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class MedicalProcedureController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public MedicalProcedureController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpGet]
    public async Task<ActionResult<List<MedicalProcedureModel>>> Get([FromQuery] GetMedicalProcedureRequest request)
    {
        var response = await _messageBus.InvokeAsync<List<MedicalProcedureModel>>(request);

        return response.ReturnOkOrNotFound();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalProcedureModel>> GetById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<MedicalProcedureModel?>(new GetMedicalProcedureByIdRequest(id));

        return response.ReturnOkOrNotFound();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount([FromQuery] GetMedicalProcedureCountRequest request)
    {
        var response = await _messageBus.InvokeAsync<int>(request);

        return response.ReturnOkOrNotFound();
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ValidationResponseModel>> Post(AddMedicalProcedureRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnCreatedOrBadRequest("medicalProcedure");
    }

    [HttpPut]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ValidationResponseModel>> Put(UpdateMedicalProcedureRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnNoContentOrBadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ValidationResponseModel>> Delete(Guid id)
    {
        var response = await _messageBus.InvokeAsync<ResponseModel>(new DeleteMedicalProcedureByIdRequest(id));

        return response.ReturnNoContentOrBadRequest();
    }
}

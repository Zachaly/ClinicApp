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
public class DrugClassController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public DrugClassController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpGet]
    public async Task<ActionResult<List<DrugClassModel>>> Get([FromQuery] GetDrugClassRequest request)
    {
        var result = await _messageBus.InvokeAsync<List<DrugClassModel>>(request);

        return ResponseModelExtensions.ReturnOkOrNotFound(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DrugClassModel>> GetById(Guid id)
    {
        var result = await _messageBus.InvokeAsync<DrugClassModel?>(new GetDrugClassByIdRequest(id));

        return ResponseModelExtensions.ReturnOkOrNotFound(result);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount([FromQuery] GetDrugClassCountRequest request)
    {
        var result = await _messageBus.InvokeAsync<int>(request);

        return ResponseModelExtensions.ReturnOkOrNotFound(result);
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ValidationResponseModel>> Post(AddDrugClassRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnCreatedOrBadRequest("drugClass/");
    }

    [HttpPut]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ValidationResponseModel>> Put(UpdateDrugClassRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnNoContentOrBadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AuthPolicyNames.RequireReceptionist)]
    public async Task<ActionResult<ResponseModel>> DeleteById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<ResponseModel>(new DeleteDrugClassByIdRequest(id));

        return response.ReturnNoContentOrBadRequest();
    }
}

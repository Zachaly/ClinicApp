using ClinicApp.Application.Handler;
using ClinicApp.Application.Model;
using ClinicApp.Domain.Request.Add;
using ClinicApp.Domain.Request.Get;
using ClinicApp.Domain.Request.Update;
using ClinicApp.Domain.Response;
using ClinicApp.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace ClinicApp.WebApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class DrugController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public DrugController(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    [HttpGet]
    public async Task<ActionResult<List<DrugModel>>> Get([FromQuery] GetDrugRequest request)
    {
        var response = await _messageBus.InvokeAsync<List<DrugModel>>(request);

        return response.ReturnOkOrNotFound();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DrugModel>> GetById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<DrugModel>(new GetDrugByIdRequest(id));

        return response.ReturnOkOrNotFound();
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount([FromQuery] GetDrugCountRequest request)
    {
        var response = await _messageBus.InvokeAsync<int>(request);

        return response.ReturnOkOrNotFound();
    }

    [HttpPost]
    public async Task<ActionResult<ValidationResponseModel>> Post(AddDrugRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnCreatedOrBadRequest("drug/");
    }

    [HttpPut]
    public async Task<ActionResult<ValidationResponseModel>> Put(UpdateDrugRequest request)
    {
        var response = await _messageBus.InvokeAsync<ValidationResponseModel>(request);

        return response.ReturnNoContentOrBadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseModel>> DeleteById(Guid id)
    {
        var response = await _messageBus.InvokeAsync<ResponseModel>(new DeleteDrugByIdRequest(id));

        return response.ReturnNoContentOrBadRequest();
    }
}

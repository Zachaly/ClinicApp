using ClinicApp.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.WebApi.Extensions;

public static class ResponseModelExtensions
{
    public static ActionResult ReturnOkOrNotFound<TResponse>(this TResponse response)
    {
        if(response is null)
        {
            return new NotFoundObjectResult(response);
        }

        return new OkObjectResult(response);
    }

    public static ActionResult ReturnOkOrBadRequest(this ResponseModel response)
    {
        if(!response.IsSuccess)
        {
            return new BadRequestObjectResult(response);
        }

        return new OkObjectResult(response);
    }

    public static ActionResult ReturnNoContentOrBadRequest(this ResponseModel response)
    {
        if(!response.IsSuccess)
        {
            return new BadRequestObjectResult(response);
        }

        return new NoContentResult();
    }

    public static ActionResult ReturnCreatedOrBadRequest(this ResponseModel response, string route)
    {
        if(!response.IsSuccess)
        {
            return new BadRequestObjectResult(response);
        }

        return new CreatedAtRouteResult(route, response);
    }
}

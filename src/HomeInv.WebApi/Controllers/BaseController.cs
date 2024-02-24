using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

public class BaseController : Controller
{
    protected ActionResult CheckErrorForOkResult(Common.ServiceContracts.BaseResponse serviceResponse)
    {
        if (serviceResponse.IsSuccessful)
        {
            return new OkResult();
        }

        return new BadRequestObjectResult(serviceResponse.Result.Messages);
    }

    protected ActionResult CheckErrorForOkResult(Common.ServiceContracts.BaseResponse serviceResponse, Contracts.BaseResponse apiResponse)
    {
        if (serviceResponse.IsSuccessful)
        {
            return new OkObjectResult(apiResponse);
        }

        return new BadRequestObjectResult(serviceResponse.Result.Messages);
    }
}
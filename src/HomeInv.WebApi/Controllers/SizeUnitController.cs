using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.SizeUnit;
using HomeInv.WebApi.Contracts.SizeUnit;
using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("size-unit")]
public class SizeUnitController : BaseController
{
    private readonly ISizeUnitService _sizeUnitService;

    public SizeUnitController(ISizeUnitService sizeUnitService)
    {
        _sizeUnitService = sizeUnitService;
    }

    //TODO: This can be cached
    [HttpGet]
    public async Task<ActionResult> GetAllAsync(
        [FromQuery] GetSizeUnitsRequest request,
        CancellationToken ct)
    {
        var serviceResponse = await _sizeUnitService.GetAllSizesAsync(new GetAllSizesRequest()
        {
            RequestUserId = request.UserId
        }, ct);
        return CheckErrorForOkResult(serviceResponse, new GetSizeUnitsResponse() { SizeUnits = serviceResponse.SizeUnits });
    }
}

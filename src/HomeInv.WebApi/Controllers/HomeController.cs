using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.WebApi.Contracts.Home;
using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("home")]
public class HomeController : Controller
{
    private readonly IHomeService _homeService;

    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult> GetAllAsync(
        [FromQuery]GetAllRequest request,
        CancellationToken ct)
    {
        var response = new GetAllResponse();
        var serviceRequest = new GetHomesOfUserRequest()
        {
            RequestUserId = request.UserId
        };
        var serviceResponse = await _homeService.GetHomesOfUserAsync(serviceRequest, ct);
        response.Homes = serviceResponse.Homes;
        return new JsonResult(response);
    }

    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync()
    {
        return new JsonResult(string.Empty);
    }
}

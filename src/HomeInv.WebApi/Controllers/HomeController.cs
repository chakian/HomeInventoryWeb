using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.WebApi.Contracts.Home;
using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("home")]
public sealed class HomeController : BaseController
{
    private readonly IHomeService _homeService;

    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(
        Contracts.Home.CreateHomeRequest request,
        CancellationToken ct)
    {
        var createHomeRequest = new Common.ServiceContracts.Home.CreateHomeRequest()
        {
            HomeEntity = new HomeEntity()
            {
                Name = request.Name,
                Description = request.Description,
            },
            RequestUserId = request.UserId,
        };
        var serviceResponse = await _homeService.CreateHomeAsync(createHomeRequest, ct);
        return CheckErrorForOkResult(serviceResponse);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult> GetAllAsync(
        [FromQuery]GetHomesRequest request,
        CancellationToken ct)
    {
        var response = new GetHomesResponse();
        var serviceRequest = new GetHomesOfUserRequest()
        {
            RequestUserId = request.UserId
        };
        var serviceResponse = await _homeService.GetHomesOfUserAsync(serviceRequest, ct);
        response.Homes = serviceResponse.Homes;
        return CheckErrorForOkResult(serviceResponse, response);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync(
        Contracts.Home.UpdateHomeRequest request,
        CancellationToken ct)
    {
        var updateHomeRequest = new Common.ServiceContracts.Home.UpdateHomeRequest()
        {
            HomeEntity = new HomeEntity()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
            },
            RequestUserId = request.UserId,
        };
        var serviceResponse = await _homeService.UpdateHomeAsync(updateHomeRequest, ct);
        return CheckErrorForOkResult(serviceResponse);
    }
}

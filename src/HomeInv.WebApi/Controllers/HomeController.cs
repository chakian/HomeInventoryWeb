using HomeInv.Common.Entities;
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
    public async Task<ActionResult> CreateAsync(CreateRequest request)
    {
        var createHomeRequest = new CreateHomeRequest()
        {
            HomeEntity = new HomeEntity()
            {
                Name = request.Name,
                Description = request.Description,
            },
            RequestUserId = request.UserId,
        };
        var serviceResponse = await _homeService.CreateHomeAsync(createHomeRequest, CancellationToken.None);
        if (!serviceResponse.IsSuccessful)
        {
            return new BadRequestObjectResult(serviceResponse.Result.Messages);
        }

        return new OkResult();
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

    [HttpPut]
    public async Task<ActionResult> UpdateAsync(UpdateRequest request)
    {
        var updateHomeRequest = new UpdateHomeRequest()
        {
            HomeEntity = new HomeEntity()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
            },
            RequestUserId = request.UserId,
        };
        var serviceResponse = await _homeService.UpdateHomeAsync(updateHomeRequest, CancellationToken.None);
        if (!serviceResponse.IsSuccessful)
        {
            return new BadRequestObjectResult(serviceResponse.Result.Messages);
        }

        return new OkResult();
    }
}

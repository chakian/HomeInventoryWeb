using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("area")]
public class AreaController : Controller
{
    [HttpPost]
    public async Task<ActionResult> CreateAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpGet]
    public async Task<ActionResult> GetAreasOfHomeAsync()
    {
        return new JsonResult(string.Empty);
    }
}

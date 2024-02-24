using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("area-user")]
public class AreaUserController : Controller
{
    [HttpPost]
    public async Task<ActionResult> InsertAreaUserAsync()
    {
        return new JsonResult(string.Empty);
    }
}

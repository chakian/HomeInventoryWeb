using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("home-user")]
public class HomeUserController : Controller
{
    [HttpPost]
    public async Task<ActionResult> CreateAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpGet]
    public async Task<ActionResult> GetUsersAsync()
    {
        return new JsonResult(string.Empty);
    }
}

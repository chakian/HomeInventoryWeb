using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("user")]
public class UserController : Controller
{
    [HttpGet]
    public async Task<ActionResult> SearchAsync()
    {
        return new JsonResult(string.Empty);
    }
}

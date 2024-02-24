using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    [HttpGet]
    public IActionResult Search()
    {
        return View();
    }
}

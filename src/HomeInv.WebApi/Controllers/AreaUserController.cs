using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("area-user")]
public class AreaUserController : Controller
{
    [HttpPost]
    public IActionResult InsertAreaUser()
    {
        return View();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AreaUserController : Controller
{
    [HttpPost]
    public IActionResult InsertAreaUser()
    {
        return View();
    }
}

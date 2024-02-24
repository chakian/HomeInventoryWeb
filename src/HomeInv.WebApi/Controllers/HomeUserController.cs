using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeUserController : Controller
{
    [HttpPost]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetUsers()
    {
        return View();
    }
}

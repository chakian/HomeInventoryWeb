using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("home-user")]
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

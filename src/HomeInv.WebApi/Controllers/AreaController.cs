using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("area")]
public class AreaController : Controller
{
    [HttpPost]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetAreasOfHome()
    {
        return View();
    }
}

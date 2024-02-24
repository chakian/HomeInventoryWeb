using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SizeUnitController : Controller
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return View();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("size-unit")]
public class SizeUnitController : Controller
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return View();
    }
}

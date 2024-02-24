using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

public class SizeUnitController : Controller
{
    public IActionResult GetAll()
    {
        return View();
    }
}

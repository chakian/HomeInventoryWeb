using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

public class AreaController : Controller
{
    public IActionResult Create()
    {
        return View();
    }

    public IActionResult GetAreasOfHome()
    {
        return View();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

public class ItemStockController : Controller
{
    public IActionResult Get()
    {
        return View();
    }

    public IActionResult GetByDefinitionIds()
    {
        return View();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("item-stock")]
public class ItemStockController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return View();
    }

    [HttpGet("get-by-definition-ids")]
    public IActionResult GetByDefinitionIds()
    {
        return View();
    }
}

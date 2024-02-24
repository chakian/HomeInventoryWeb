using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemStockController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return View();
    }

    [HttpGet]
    [Route("[controller]/Get_ByDefIds")]
    public IActionResult GetByDefinitionIds()
    {
        return View();
    }
}

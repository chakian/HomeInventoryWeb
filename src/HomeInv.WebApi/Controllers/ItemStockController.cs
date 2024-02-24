using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("item-stock")]
public class ItemStockController : Controller
{
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpGet("get-by-definition-ids")]
    public async Task<ActionResult> GetByDefinitionIdsAsync()
    {
        return new JsonResult(string.Empty);
    }
}

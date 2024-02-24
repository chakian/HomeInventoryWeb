using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("size-unit")]
public class SizeUnitController : Controller
{
    [HttpGet]
    public async Task<ActionResult> GetAllAsync()
    {
        return new JsonResult(string.Empty);
    }
}

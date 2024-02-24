using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("category")]
public class CategoryController : Controller
{
    [HttpPost]
    public async Task<ActionResult> CreateAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpGet("get-hierarchial")]
    public async Task<ActionResult> GetHierarchialAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpGet("get-ordered")]
    public async Task<ActionResult> GetOrderedAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync()
    {
        return new JsonResult(string.Empty);
    }
}

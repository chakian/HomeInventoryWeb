using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("home")]
public class HomeController : Controller
{
    [HttpPost]
    public async Task<ActionResult> CreateAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpGet("get-all")]
    public async Task<ActionResult> GetAllAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync()
    {
        return new JsonResult(string.Empty);
    }
}

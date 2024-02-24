using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("user-setting")]
public class UserSettingController : Controller
{
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpPost]
    public async Task<ActionResult> InsertOrUpdateForDefaultHomeAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync()
    {
        return new JsonResult(string.Empty);
    }
}

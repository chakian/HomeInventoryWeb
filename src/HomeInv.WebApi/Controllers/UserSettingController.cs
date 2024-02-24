using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("user-setting")]
public class UserSettingController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return View();
    }

    [HttpPost]
    public IActionResult InsertOrUpdateForDefaultHome()
    {
        return View();
    }

    [HttpPut]
    public IActionResult Update()
    {
        return View();
    }
}

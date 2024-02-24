using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
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

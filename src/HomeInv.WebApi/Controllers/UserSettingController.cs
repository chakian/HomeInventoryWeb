using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

public class UserSettingController : Controller
{
    public IActionResult Get()
    {
        return View();
    }

    public IActionResult InsertOrUpdateForDefaultHome()
    {
        return View();
    }

    public IActionResult Update()
    {
        return View();
    }
}

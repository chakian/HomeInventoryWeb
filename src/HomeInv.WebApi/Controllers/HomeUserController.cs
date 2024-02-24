using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

public class HomeUserController : Controller
{
    public IActionResult Create()
    {
        return View();
    }

    public IActionResult GetUsers()
    {
        return View();
    }
}

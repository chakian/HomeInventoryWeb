using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

public class UserController : Controller
{
    public IActionResult Search()
    {
        return View();
    }
}

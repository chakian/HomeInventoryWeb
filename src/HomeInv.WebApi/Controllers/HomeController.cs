using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("home")]
public class HomeController : Controller
{
    [HttpPost]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet("get-all")]
    public IActionResult GetAll()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Get()
    {
        return View();
    }

    [HttpPut]
    public IActionResult Update()
    {
        return View();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : Controller
{
    [HttpPost]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    [Route("[controller]/Get_All")]
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

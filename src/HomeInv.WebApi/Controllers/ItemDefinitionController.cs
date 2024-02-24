using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemDefinitionController : Controller
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
    [Route("[controller]/Get_Filtered")]
    public IActionResult GetFiltered()
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

    [HttpDelete]
    public IActionResult Delete()
    {
        return View();
    }
}

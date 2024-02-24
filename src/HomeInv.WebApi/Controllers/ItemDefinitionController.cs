using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("item-definition")]
public class ItemDefinitionController : Controller
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

    [HttpGet("get-filtered")]
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

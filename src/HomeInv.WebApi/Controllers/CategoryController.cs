using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("category")]
public class CategoryController : Controller
{
    [HttpPost]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet("get-hierarchial")]
    public IActionResult GetHierarchial()
    {
        return View();
    }

    [HttpGet("get-ordered")]
    public IActionResult GetOrdered()
    {
        return View();
    }

    [HttpPut]
    public IActionResult Update()
    {
        return View();
    }
}

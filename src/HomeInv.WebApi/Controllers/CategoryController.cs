using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : Controller
{
    [HttpPost]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    [Route("[controller]/Get_Hierarchial")]
    public IActionResult GetHierarchial()
    {
        return View();
    }

    [HttpGet]
    [Route("[controller]/Get_Ordered")]
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

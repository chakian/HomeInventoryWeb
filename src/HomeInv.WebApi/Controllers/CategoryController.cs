using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

public class CategoryController : Controller
{
    public IActionResult Create()
    {
        return View();
    }

    public IActionResult GetHierarchial()
    {
        return View();
    }

    public IActionResult GetOrdered()
    {
        return View();
    }

    public IActionResult Update()
    {
        return View();
    }
}

using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.WebApi.Contracts.Category;
using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("category")]
public sealed class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync()
    {
        return new JsonResult(string.Empty);
    }

    [HttpGet("get-hierarchical")]
    public async Task<ActionResult> GetHierarchicalAsync(
        GetAllRequest request, 
        CancellationToken ct)
    {
        var serviceRequest = new GetCategoriesOfHomeRequest()
        {
            HomeId = request.HomeId,
            RequestUserId = request.UserId,
        };
        var categoriesResponse = _categoryService.GetCategoriesOfHome_HierarchicalAsync(serviceRequest, ct).Result;
        return new OkObjectResult(new GetAllResponse()
        {
            Categories = categoriesResponse.Categories,
        });
    }

    [HttpGet("get-ordered")]
    public async Task<ActionResult> GetOrderedAsync(
        GetAllRequest request,
        CancellationToken ct)
    {
        var serviceResponse = await _categoryService.GetCategoriesOfHome_OrderedAsync(new GetCategoriesOfHomeRequest()
        {
            HomeId = request.HomeId
        }, ct);
        var categoryList = serviceResponse.Categories;
        return new JsonResult(string.Empty);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync()
    {
        return new JsonResult(string.Empty);
    }
}

using HomeInv.Common.Entities;
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
    public async Task<ActionResult> CreateAsync(
        Contracts.Category.CreateCategoryRequest request,
        CancellationToken ct)
    {
        var createCategoryRequest = new Common.ServiceContracts.Category.CreateCategoryRequest()
        {
            CategoryEntity = new CategoryEntity()
            {
                Name = request.Name,
                Description = request.Description,
                ParentCategoryId = request.ParentCategoryId,
            },
            HomeId = request.HomeId,
            RequestUserId = request.UserId,
        };
        var serviceResponse = await _categoryService.CreateCategoryAsync(createCategoryRequest, ct);
        return CheckErrorForOkResult(serviceResponse);
    }

    [HttpGet("get-hierarchical")]
    public async Task<ActionResult> GetHierarchicalAsync(
        [FromQuery] GetCategoriesRequest request, 
        CancellationToken ct)
    {
        var serviceRequest = new GetCategoriesOfHomeRequest()
        {
            HomeId = request.HomeId,
            RequestUserId = request.UserId,
        };
        var categoriesResponse = _categoryService.GetCategoriesOfHome_HierarchicalAsync(serviceRequest, ct).Result;
        var response = new GetCategoriesResponse()
        {
            Categories = categoriesResponse.Categories,
        };
        return CheckErrorForOkResult(categoriesResponse, response);
    }

    [HttpGet("get-ordered")]
    public async Task<ActionResult> GetOrderedAsync(
        [FromQuery] GetCategoriesRequest request,
        CancellationToken ct)
    {
        var serviceResponse = await _categoryService.GetCategoriesOfHome_OrderedAsync(new GetCategoriesOfHomeRequest()
        {
            HomeId = request.HomeId
        }, ct);
        var response = new GetCategoriesResponse()
        {
            Categories = serviceResponse.Categories,
        };
        return CheckErrorForOkResult(serviceResponse, response);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync(
        [FromBody] Contracts.Category.UpdateCategoryRequest request,
        CancellationToken ct)
    {
        var updateCategoryRequest = new Common.ServiceContracts.Category.UpdateCategoryRequest()
        {
            CategoryId = request.Id,
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = request.ParentCategoryId,
            RequestUserId = request.UserId
        };
        var serviceResponse = await _categoryService.UpdateCategoryAsync(updateCategoryRequest, ct);
        return CheckErrorForOkResult(serviceResponse);
    }
}

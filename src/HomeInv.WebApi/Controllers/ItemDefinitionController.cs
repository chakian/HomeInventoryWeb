using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.WebApi.Contracts.ItemDefinition;
using Microsoft.AspNetCore.Mvc;
using CreateItemDefinitionRequest = HomeInv.WebApi.Contracts.ItemDefinition.CreateItemDefinitionRequest;
using DeleteItemDefinitionRequest = HomeInv.WebApi.Contracts.ItemDefinition.DeleteItemDefinitionRequest;
using UpdateItemDefinitionRequest = HomeInv.WebApi.Contracts.ItemDefinition.UpdateItemDefinitionRequest;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("item-definition")]
public class ItemDefinitionController : BaseController
{
    private readonly IItemDefinitionService _itemDefinitionService;

    public ItemDefinitionController(IItemDefinitionService itemDefinitionService)
    {
        _itemDefinitionService = itemDefinitionService;
    }

    [HttpGet("get-all")]
    public async Task<ActionResult> GetAllAsync(
        [FromQuery] GetItemDefinitionsRequest request,
        CancellationToken ct)
    {
        var serviceRequest = new GetAllItemDefinitionsInHomeRequest()
        {
            IncludeInactive = request.IncludeInactive,
            HomeId = request.HomeId,
            RequestUserId = request.UserId
        };
        var serviceResponse = await _itemDefinitionService.GetAllItemDefinitionsInHomeAsync(serviceRequest, ct);
        return CheckErrorForOkResult(serviceResponse, new GetItemDefinitionsResponse() { ItemDefinitions = serviceResponse.Items });
    }

    [HttpGet("get-filtered")]
    public async Task<ActionResult> GetFilteredAsync(
        [FromQuery] GetItemDefinitionsRequest request,
        CancellationToken ct)
    {
        var serviceRequest = new GetFilteredItemDefinitionsInHomeRequest()
        {
            CategoryId = request.CategoryId,
            AreaId = request.AreaId,
            HomeId = request.HomeId,
            RequestUserId = request.UserId,
        };
        var serviceResponse = await _itemDefinitionService.GetFilteredItemDefinitionsInHomeAsync(serviceRequest, ct);
        return CheckErrorForOkResult(serviceResponse, new GetItemDefinitionsResponse() { ItemDefinitions = serviceResponse.Items });
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(
        CreateItemDefinitionRequest request,
        CancellationToken ct)
    {
        var createRequest = new Common.ServiceContracts.ItemDefinition.CreateItemDefinitionRequest()
        {
            ItemEntity = new ItemDefinitionEntity()
            {
                Name = request.Name,
                Description = request.Description,
                SizeUnitId = request.SizeUnitId,
                CategoryId = request.CategoryId,
                IsExpirable = request.IsExpirable,
            },
            ImageBase64 = request.ImageBase64,
            HomeId = request.HomeId,
            RequestUserId = request.UserId,
        };
        var serviceResponse = await _itemDefinitionService.CreateItemDefinitionAsync(createRequest, ct);
        return CheckErrorForOkResult(serviceResponse);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync(
        UpdateItemDefinitionRequest request,
        CancellationToken ct)
    {
        var updateRequest = new Common.ServiceContracts.ItemDefinition.UpdateItemDefinitionRequest()
        {
            ItemDefinitionId = request.Id,
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            IsExpirable = request.IsExpirable,
            ImageBase64 = request.ImageBase64,
            HomeId = request.HomeId,
            RequestUserId = request.UserId,
        };
        var serviceResponse = await _itemDefinitionService.UpdateItemDefinitionAsync(updateRequest, ct);
        return CheckErrorForOkResult(serviceResponse);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync(
        DeleteItemDefinitionRequest request,
        CancellationToken ct)
    {
        var deleteRequest = new Common.ServiceContracts.ItemDefinition.DeleteItemDefinitionRequest()
        {
            ItemDefinitionId = request.Id,
            HomeId = request.HomeId,
            RequestUserId = request.UserId,
        };
        var serviceResponse = await _itemDefinitionService.DeleteItemDefinitionAsync(deleteRequest, ct);
        return CheckErrorForOkResult(serviceResponse);
    }
}

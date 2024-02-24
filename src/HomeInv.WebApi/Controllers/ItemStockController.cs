using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts;
using HomeInv.WebApi.Contracts.ItemStock;
using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("item-stock")]
public class ItemStockController : BaseController
{
    private readonly IItemStockService _itemStockService;
    
    public ItemStockController(IItemStockService itemStockService)
    {
        _itemStockService = itemStockService;
    }

    [HttpGet("get-by-definition-ids")]
    public async Task<ActionResult> GetByDefinitionIdsAsync(
        [FromQuery] GetItemStocksByItemDefinitionIdsRequest request,
        CancellationToken ct)
    {
        var serviceResponse = await _itemStockService.GetItemStocksByItemDefinitionIdsAsync(new Common.ServiceContracts.ItemStock.GetItemStocksByItemDefinitionIdsRequest()
        {
            HomeId = request.HomeId,
            ItemDefinitionIdList = request.ItemDefinitionIds,
            RequestUserId = request.UserId
        }, ct);
        return CheckErrorForOkResult(serviceResponse, new GetItemStocksResponse() { ItemStocks = serviceResponse.ItemStocks });
    }

    [HttpGet("get-by-home-id")]
    public async Task<ActionResult> GetByHomeIdAsync(
        [FromQuery] GetItemStockStatusByHomeIdRequest request,
        CancellationToken ct)
    {
        var stockDictionary = await _itemStockService.CheckStocksPrepareShoppingListAndSendEmailAsync(request.HomeId, ct);
        return CheckErrorForOkResult(new BaseResponse(), new GetItemStockStatusByHomeIdResponse() { StockStatus = stockDictionary[request.HomeId] });
    }
}

using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class StockStatusController : AuthenticatedControllerBase
{
    private readonly IItemDefinitionService _itemDefinitionService;
    private readonly IItemStockService _itemStockService;

    public StockStatusController(HomeInventoryDbContext dbContext,
        IItemDefinitionService itemDefinitionService,
        IItemStockService itemStockService) : base(dbContext)
    {
        _itemDefinitionService = itemDefinitionService;
        _itemStockService = itemStockService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var definitions = _itemDefinitionService.GetFilteredItemDefinitionsInHome(new GetFilteredItemDefinitionsInHomeRequest()
        {
            HomeId = _defaultHomeId,
            AreaId = 0,
            CategoryId = 0,
            RequestUserId = _userId
        });

        var stocks = _itemStockService.GetItemStocksByItemDefinitionIds(new GetItemStocksByItemDefinitionIdsRequest()
        {
            HomeId = _defaultHomeId,
            ItemDefinitionIdList = definitions.Items.Select(def => def.Id).ToList(),
            RequestUserId = _userId
        }).ItemStocks;

        var resultList = new ItemStockListDto();

        foreach (var definition in definitions.Items)
        {
            var itemStock = new ItemStockDto
            {
                ItemDefinitionId = definition.Id,
                ItemName = definition.Name,
                ItemDescription = definition.Description,
                CategoryId = definition.CategoryId,
                CategoryName = definition.CategoryName,
                CategoryParentNames = definition.CategoryFullName,
                ImageName = definition.ImageName,
                SizeUnitId = definition.SizeUnitId,
                SizeUnitName = definition.SizeUnitName
            };
            var currentItemStocks = stocks.Where(s => s.ItemDefinitionId == definition.Id).ToList();
            if (currentItemStocks.Any())
            {
                foreach (var stock in currentItemStocks)
                {
                    var stockOverview = itemStock.Clone();
                    stockOverview.StockId = stock.Id;
                    stockOverview.CurrentStockAmount = stock.Quantity;
                    stockOverview.ExpirationDate = definition.IsExpirable ? stock.ExpirationDate : DateTime.MinValue;
                    resultList.ItemStocks.Add(stockOverview);
                }
            }
            else
            {
                resultList.ItemStocks.Add(itemStock);
            }
        }

        return new JsonResult(resultList);
    }
}

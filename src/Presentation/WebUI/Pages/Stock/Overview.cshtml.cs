using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WebUI.Base;

namespace WebUI.Pages.Stock
{
    public class OverviewModel : BaseAuthenticatedPageModel<OverviewModel>
    {
        readonly IItemDefinitionService _itemDefinitionService;
        readonly IItemStockService _itemStockService;

        public OverviewModel(ILogger<OverviewModel> logger,
            HomeInventoryDbContext dbContext,
            IItemDefinitionService itemDefinitionService,
            IItemStockService itemStockService) : base(logger, dbContext)
        {
            _itemDefinitionService = itemDefinitionService;
            _itemStockService = itemStockService;
        }

        [BindProperty]
        public List<ItemStockOverview> OverviewList { get; set; }

        [BindProperty]
        public int HomeId { get; set; }

        public void OnGet()
        {
            HomeId = UserSettings.DefaultHomeId;

            var definitions = _itemDefinitionService.GetFilteredItemDefinitionsInHome(new GetFilteredItemDefinitionsInHomeRequest()
            {
                HomeId = HomeId,
                AreaId = 0,
                CategoryId = 0,
                RequestUserId = UserId
            });

            var stocks = _itemStockService.GetItemStocksByItemDefinitionIds(new GetItemStocksByItemDefinitionIdsRequest()
            {
                HomeId = HomeId,
                ItemDefinitionIdList = definitions.Items.Select(def => def.Id).ToList(),
                RequestUserId = UserId
            });

            OverviewList = new List<ItemStockOverview>();

            foreach (var definition in definitions.Items)
            {
                var overview = new ItemStockOverview
                {
                    ItemDefinitionId = definition.Id,
                    ItemName = definition.Name,
                    ItemDescription = definition.Description,
                    CategoryId = definition.CategoryId,
                    CategoryName = definition.CategoryName,
                    CategoryParentNames = definition.CategoryFullName,
                    ImageName = definition.ImageName,
                    
                };
                var currentItemStocks = stocks.ItemStocks.Where(s => s.ItemDefinitionId == definition.Id).ToList();
                if (currentItemStocks.Any())
                {
                    foreach (var stock in currentItemStocks)
                    {
                        var stockOverview = overview.Clone();
                        stockOverview.StockId= stock.Id;
                        stockOverview.AreaId = stock.AreaId;
                        stockOverview.AreaName= stock.AreaName;
                        stockOverview.CurrentStockAmount = stock.Quantity;
                        stockOverview.ExpirationDate= stock.ExpirationDate;
                        stockOverview.SizeUnitId = stock.SizeUnitId;
                        stockOverview.SizeUnitName = stock.SizeUnitName;
                        OverviewList.Add(stockOverview);
                    }
                }
                else
                {
                    overview.AreaName = "-";
                    OverviewList.Add(overview);
                }
            }
        }
    }

    public class ItemStockOverview
    {
        public int StockId { get; set; }
        public int ItemDefinitionId { get; set; }
        public string ImageName { get; set; }

        public int AreaId { get; set; }
        public string AreaName { get; set; }

        public int SizeUnitId { get; set; }
        public string SizeUnitName { get; set; }

        public string ItemName { get; set; }
        public string ItemDescription { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryParentNames { get; set; }

        public DateTime ExpirationDate { get; set; }
        public decimal CurrentStockAmount { get; set; }

        public ItemStockOverview Clone()
        {
            return new ItemStockOverview()
            {
                StockId = StockId,
                ItemDefinitionId = ItemDefinitionId,
                ImageName = ImageName,
                AreaId = AreaId,
                AreaName = AreaName,
                SizeUnitId = SizeUnitId,
                SizeUnitName = SizeUnitName,
                ItemName = ItemName,
                ItemDescription = ItemDescription,
                CategoryId = CategoryId,
                CategoryName = CategoryName,
                CategoryParentNames = CategoryParentNames,
                ExpirationDate = ExpirationDate,
                CurrentStockAmount = CurrentStockAmount,
            };
        }
    }
}

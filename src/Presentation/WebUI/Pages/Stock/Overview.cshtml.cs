using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WebUI.Base;

namespace WebUI.Pages.Stock
{
    public class OverviewModel : BaseAuthenticatedPageModel<OverviewModel>
    {
        public OverviewModel(ILogger<OverviewModel> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }

        [BindProperty]
        public List<ItemStockOverview> OverviewList { get; set; }

        [BindProperty]
        public int HomeId { get; set; }

        public void OnGet()
        {
            HomeId = UserSettings.DefaultHomeId;
            OverviewList = new List<ItemStockOverview>();
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
    }
}

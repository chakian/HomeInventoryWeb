using HomeInv.Common.Constants;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Area;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Common.ServiceContracts.SizeUnit;
using HomeInv.Language;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebUI.Base;

namespace WebUI.Pages.Stock
{
    public class CreateStep2Model : BaseAuthenticatedPageModel<CreateStep2Model>
    {
        readonly IAreaService _areaService;
        readonly ISizeUnitService _sizeUnitService;
        readonly IUpdateItemStockHandler _updateItemStockHandler;

        public CreateStep2Model(ILogger<CreateStep2Model> logger,
            HomeInventoryDbContext dbContext,
            IAreaService areaService,
            ISizeUnitService sizeUnitService,
            IUpdateItemStockHandler updateItemStockHandler) : base(logger, dbContext)
        {
            _areaService = areaService;
            _sizeUnitService = sizeUnitService;
            _updateItemStockHandler = updateItemStockHandler;
        }

        [BindProperty]
        public int ItemDefinitionId { get; set; }

        [BindProperty]
        public ItemStockEntry StockEntry { get; set; }

        [BindProperty]
        public int HomeId { get; set; }

        [BindProperty]
        public List<SelectListItem> AllAreas { get; set; }

        [BindProperty]
        public List<SelectListItem> AllSizeUnits { get; set; }

        [BindProperty]
        public List<SelectListItem> StockActionTypes { get; set; }

        public void OnGet()
        {
            if (int.TryParse(Request.Query["ItemDefinitionId"].ToString(), out int _itemDefinitionId))
            {
                ItemDefinitionId = _itemDefinitionId;
                //TODO: Get ItemDefinition info here
                StockEntry = new ItemStockEntry()
                {
                    ActionDate = DateTime.Now,
                    ExpirationDate = DateTime.Now,
                };
                HomeId = UserSettings.DefaultHomeId;

                StockActionTypes = new List<SelectListItem>() {
                    new SelectListItem(){ Text = "Satin Alindi", Value = ((int)ItemStockActionTypeEnum.Purchased).ToString(), Selected = true },
                    new SelectListItem(){ Text = "Hediye Geldi", Value = ((int)ItemStockActionTypeEnum.GiftedIn).ToString() }
                };

                AllAreas = new List<SelectListItem>
                {
                    new SelectListItem { Text = "-- Oda --", Value = "0", Selected = true }
                };
                var areas = _areaService.GetAreasOfHome(new GetAreasOfHomeRequest()
                {
                    HomeId = UserSettings.DefaultHomeId,
                    RequestUserId = UserId
                });
                foreach (var area in areas.Areas)
                {
                    AllAreas.Add(new SelectListItem()
                    {
                        Text = area.Name,
                        Value = area.Id.ToString()
                    });
                }

                AllSizeUnits = new List<SelectListItem>() { new SelectListItem() { Text = "-- Boyut --", Value = "0" } };
                var sizes = _sizeUnitService.GetAllSizes(new GetAllSizesRequest() { RequestUserId = UserId });
                foreach (var size in sizes.SizeUnits)
                {
                    AllSizeUnits.Add(new SelectListItem()
                    {
                        Text = size.Name,
                        Value = size.Id.ToString()
                    });
                }
            }
            else
            {
                SetErrorMessage("Urun seciminde bir sorun oldu sanki. Tekrar dener misiniz?");
                RedirectToPage("CreateStep1");
            }
        }

        protected override IActionResult OnModelPost()
        {
            var request = new UpdateItemStockRequest()
            {
                HomeId = UserSettings.DefaultHomeId,
                ItemStockActionTypeId = StockEntry.StockActionTypeId,
                ItemDefinitionId = ItemDefinitionId,
                AreaId = StockEntry.AreaId,
                SizeUnitId = StockEntry.SizeUnitId,
                Size = StockEntry.EntryAmount,
                ExpirationDate = StockEntry.ExpirationDate,
                ActionDate = StockEntry.ActionDate,
                ActionTarget = StockEntry.ActionTarget,
                Price = StockEntry.TotalPrice
            };

            CallService(_updateItemStockHandler.Execute, request);

            return RedirectToPage("Overview");
        }
    }

    public class ItemStockEntry
    {
        public string ImageName { get; set; }

        [Display(Name = nameof(Resources.ItemStockEntry_StockActionTypeId), ResourceType = typeof(Resources))]
        [Required]
        public int StockActionTypeId { get; set; }

        [Display(Name = nameof(Resources.ItemStockEntry_AreaId), ResourceType = typeof(Resources))]
        [Required]
        public int AreaId { get; set; }

        [Display(Name = nameof(Resources.ItemStockEntry_SizeUnitId), ResourceType = typeof(Resources))]
        [Required]
        public int SizeUnitId { get; set; }

        [Display(Name = nameof(Resources.ItemStockEntry_EntryAmount), ResourceType = typeof(Resources))]
        [Required]
        public decimal EntryAmount { get; set; }

        [Display(Name = nameof(Resources.ItemStockEntry_ExpirationDate), ResourceType = typeof(Resources))]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = nameof(Resources.ItemStockEntry_ActionDate), ResourceType = typeof(Resources))]
        [Required]
        public DateTime ActionDate { get; set; }

        [Display(Name = nameof(Resources.ItemStockEntry_ActionTarget), ResourceType = typeof(Resources))]
        public string ActionTarget { get; set; }

        [Display(Name = nameof(Resources.ItemStockEntry_TotalPrice), ResourceType = typeof(Resources))]
        public decimal TotalPrice { get; set; }
    }
}

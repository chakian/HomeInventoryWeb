using HomeInv.Business.Handlers;
using HomeInv.Common.Constants;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Area;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebUI.Base;

namespace WebUI.Pages.Stock
{
    public class UpdateRemainingStockModel : BaseAuthenticatedPageModel<UpdateRemainingStockModel>
    {
        readonly IItemDefinitionService _itemDefinitionService;
        readonly IItemStockService _itemStockService;
        readonly IUpdateRemainingStockAmountHandler _updateRemainingStockAmountHandler;
        public UpdateRemainingStockModel(ILogger<UpdateRemainingStockModel> logger, 
            HomeInventoryDbContext dbContext,
            IItemDefinitionService itemDefinitionService,
            IItemStockService itemStockService,
            IUpdateRemainingStockAmountHandler updateRemainingStockAmountHandler) : base(logger, dbContext)
        {
            _itemDefinitionService = itemDefinitionService;
            _itemStockService = itemStockService;
            _updateRemainingStockAmountHandler = updateRemainingStockAmountHandler;
        }

        [BindProperty] public int ItemStockId { get; set; }
        [Display(Name = "Mevcutta Kalan Miktar")]
        [BindProperty] public decimal RemainingAmount { get; set; }
        [BindProperty] public string SizeUnitName { get; set; }
        [Display(Name = "Stok Güncelleme Tipi")]
        [BindProperty] public int StockActionTypeId { get; set; }
        [BindProperty] public List<SelectListItem> StockActionTypes { get; set; }
        [Display(Name = "Tarih")]
        [BindProperty] public DateTime ActionDate { get; set; }
        [Display(Name = "Market falan")]
        [BindProperty] public string ActionTarget { get; set; }

        public void OnGet()
        {
            bool checksPassed = true;
            if (!int.TryParse(Request.Query["ItemStockId"].ToString(), out int _itemStockId))
            {
                checksPassed = false;
                SetErrorMessage("Urun seciminde bir sorun oldu sanki. Tekrar dener misiniz?");
                RedirectToPage("Overview");
            }

            if (checksPassed) {
                ItemStockId = _itemStockId;
                ActionDate = DateTime.Now;
                ActionTarget = "";
                StockActionTypes = new List<SelectListItem>() {
                    new SelectListItem(){ Text = "Tuketildi", Value = ((int)ItemStockActionTypeEnum.Consumed).ToString(), Selected = true },
                    new SelectListItem(){ Text = "Satildi", Value = ((int)ItemStockActionTypeEnum.Sold).ToString() },
                    new SelectListItem(){ Text = "Atildi", Value = ((int)ItemStockActionTypeEnum.Dismissed).ToString() },
                    new SelectListItem(){ Text = "Kirildi", Value = ((int)ItemStockActionTypeEnum.Broken).ToString() },
                    new SelectListItem(){ Text = "Kayboldu", Value = ((int)ItemStockActionTypeEnum.Lost).ToString() },
                    new SelectListItem(){ Text = "Hediye Edildi", Value = ((int)ItemStockActionTypeEnum.GiftedOut).ToString() },
                };

                var stock = _itemStockService.GetSingleItemStock(new GetSingleItemStockRequest()
                {
                    ItemStockId = ItemStockId,
                    RequestUserId = UserId
                }).Stock;
                RemainingAmount = stock.Quantity;

                var itemDefinition = _itemDefinitionService.GetItemDefinition(new GetItemDefinitionRequest()
                {
                    ItemDefinitionId = stock.ItemDefinitionId,
                    HomeId = UserSettings.DefaultHomeId,
                    RequestUserId = UserId
                }).ItemDefinition;

                SizeUnitName = itemDefinition.SizeUnitName;
            }
        }

        protected override IActionResult OnModelPost()
        {
            var request = new UpdateRemainingStockAmountRequest()
            {
                ItemStockId = ItemStockId,
                ItemStockActionTypeId = StockActionTypeId,
                RemainingAmount = RemainingAmount,
                ActionDate = ActionDate,
                ActionTarget = ActionTarget,
                RequestUserId = UserId
            };

            CallService(_updateRemainingStockAmountHandler.Execute, request);

            return RedirectToPage("Overview");
        }
    }
}

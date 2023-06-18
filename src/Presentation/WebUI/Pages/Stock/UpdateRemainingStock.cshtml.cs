using HomeInv.Common.Constants;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Common.ServiceContracts.ItemStock;
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

        [BindProperty] public string ItemName { get; set; }
        [BindProperty] public int ItemStockId { get; set; }
        [Display(Name = "Mevcutta Kalan Miktar")]
        [RegularExpression("\\d+(,\\d+)?", ErrorMessageResourceName = nameof(Resources.Error_DecimalsShouldBeSeparatedByComma), ErrorMessageResourceType = typeof(Resources))]
        [BindProperty] public string RemainingAmount { get; set; }
        [RegularExpression("\\d+(,\\d+)?", ErrorMessageResourceName = nameof(Resources.Error_DecimalsShouldBeSeparatedByComma), ErrorMessageResourceType = typeof(Resources))]
        [BindProperty] public string ConsumedAmount { get; set; }
        [RegularExpression("\\d+(,\\d+)?", ErrorMessageResourceName = nameof(Resources.Error_DecimalsShouldBeSeparatedByComma), ErrorMessageResourceType = typeof(Resources))]
        [BindProperty] public string EnteredAmount { get; set; }
        [RegularExpression("\\d+(,\\d+)?", ErrorMessageResourceName = nameof(Resources.Error_DecimalsShouldBeSeparatedByComma), ErrorMessageResourceType = typeof(Resources))]
        [BindProperty] public string Price { get; set; }
        [BindProperty] public string SizeUnitName { get; set; }
        [Display(Name = "Stok Güncelleme Tipi")]
        [BindProperty] public int StockActionTypeId { get; set; }
        [BindProperty] public List<SelectListItem> StockActionTypes { get; set; }
        [Display(Name = "Tarih")]
        [BindProperty] public DateTime ActionDate { get; set; }
        [Display(Name = "Market falan")]
        [BindProperty] public string ActionTarget { get; set; }

        public IActionResult OnGet()
        {
            if (!int.TryParse(Request.Query["ItemStockId"].ToString(), out int _itemStockId) || _itemStockId == 0)
            {
                SetErrorMessage("Urun seciminde bir sorun oldu sanki. Tekrar dener misiniz?");
                return RedirectToPage("Overview");
            }
            else
            {
                ItemStockId = _itemStockId;

                InitializePage();

                return Page();
            }
        }

        private void InitializePage()
        {
            ActionDate = DateTime.UtcNow;
            ActionTarget = "";
            StockActionTypes = new List<SelectListItem>() {
                    new SelectListItem(){ Text = "Tuketildi", Value = ((int)ItemStockActionTypeEnum.Consumed).ToString(), Selected = true },
                    new SelectListItem(){ Text = "Satin Alindi", Value = ((int)ItemStockActionTypeEnum.Purchased).ToString(), Selected = true },
                    new SelectListItem(){ Text = "Atildi", Value = ((int)ItemStockActionTypeEnum.Dismissed).ToString() },
                    new SelectListItem(){ Text = "Kayboldu", Value = ((int)ItemStockActionTypeEnum.Lost).ToString() },
                    new SelectListItem(){ Text = "Kirildi", Value = ((int)ItemStockActionTypeEnum.Broken).ToString() },
                    new SelectListItem(){ Text = "Satildi", Value = ((int)ItemStockActionTypeEnum.Sold).ToString() },
                    new SelectListItem(){ Text = "Hediye Edildi", Value = ((int)ItemStockActionTypeEnum.GiftedOut).ToString() },
                    new SelectListItem(){ Text = "Hediye Olarak Geldi", Value = ((int)ItemStockActionTypeEnum.GiftedIn).ToString() },
                };

            var stock = _itemStockService.GetSingleItemStock(new GetSingleItemStockRequest()
            {
                ItemStockId = ItemStockId,
                RequestUserId = UserId
            }).Stock;
            RemainingAmount = stock.Quantity.ToString();

            var itemDefinition = _itemDefinitionService.GetItemDefinition(new GetItemDefinitionRequest()
            {
                ItemDefinitionId = stock.ItemDefinitionId,
                HomeId = UserSettings.DefaultHomeId,
                RequestUserId = UserId
            }).ItemDefinition;

            ItemName = itemDefinition.Name + (string.IsNullOrEmpty(itemDefinition.Description) ? "" : " (" + itemDefinition.Description + ")");
            SizeUnitName = itemDefinition.SizeUnitName;
        }

        protected override IActionResult OnModelPost()
        {
            //TODO: Eger hata gelir ve ekran tekrar acilirsa accordion'un ilgili kismi acik gelsin
            decimal.TryParse(RemainingAmount, out decimal remainingAmount);
            decimal.TryParse(ConsumedAmount, out decimal consumedAmount);
            decimal.TryParse(EnteredAmount, out decimal enteredAmount);
            decimal.TryParse(Price, out decimal price);

            var request = new UpdateRemainingStockAmountRequest()
            {
                ItemStockId = ItemStockId,
                ItemStockActionTypeId = StockActionTypeId,
                RemainingAmount = remainingAmount,
                ConsumedAmount = consumedAmount,
                EntryAmount = enteredAmount,
                Price = price,
                ActionDate = ActionDate,
                ActionTarget = ActionTarget,
                RequestUserId = UserId
            };

            var response = CallService(_updateRemainingStockAmountHandler.Execute, request);

            if (response.IsSuccessful)
            {
                return RedirectToPage("Overview");
            }
            else
            {
                InitializePage();
                return Page();
            }
        }
    }
}

using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.SizeUnit;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebUI.Base;

namespace WebUI.Pages.ShopList
{
    public class AddItemToListModel : AuthenticatedPageModelBase<AddItemToListModel>
    {
        readonly ISizeUnitService _sizeUnitService;
        public AddItemToListModel(ILogger<AddItemToListModel> logger, HomeInventoryDbContext dbContext,
            ISizeUnitService sizeUnitService) : base(logger, dbContext)
        {
            _sizeUnitService = sizeUnitService;
        }

        public string ListName { get; set; }

        [BindProperty]
        public int ListId { get; set; }

        [BindProperty]
        public bool AddAnother { get; set; }

        [BindProperty]
        public string ItemName { get; set; }

        [BindProperty]
        [RegularExpression("\\d+(,\\d+)?", ErrorMessageResourceName = nameof(Resources.Error_DecimalsShouldBeSeparatedByComma), ErrorMessageResourceType = typeof(Resources))]
        public string Amount { get; set; }

        [BindProperty]
        public int SizeId { get; set; }

        public List<SelectListItem> AllSizeUnits { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || dbContext.ShoppingLists == null)
            {
                return NotFound();
            }

            var shoppinglist = await dbContext.ShoppingLists
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (shoppinglist == null)
            {
                return NotFound();
            }

            ListId = shoppinglist.Id;

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var shoppinglist = await dbContext.ShoppingLists.FindAsync(ListId);
            if (shoppinglist == null)
            {
                return Page();
            }

            decimal _amount = decimal.Parse(Amount);
            ShoppingListItem item = new ShoppingListItem
            {
                ShoppingListId = ListId,
                ItemName = ItemName,
                Amount = _amount,
                SizeUnitId = SizeId,
                InsertUserId = UserId,
                InsertTime = DateTime.UtcNow,
                IsActive = true
            };
            dbContext.Entry(item).State= EntityState.Added;
            await dbContext.SaveChangesAsync();

            var routeValues = new
            {
                id = ListId
            };
            if (AddAnother)
            {
                return RedirectToPage("./AddItemToList", routeValues);
            }

            return RedirectToPage("./EditList", routeValues);
        }
    }
}

using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Base;

namespace WebUI.Pages.ShopList
{
    public class EditListModel : AuthenticatedPageModelBase<EditListModel>
    {
        public EditListModel(ILogger<EditListModel> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }

        [BindProperty]
        public int ShoppingListId { get; set; }
        [BindProperty]
        public ShoppingListItem[] ListItems { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || dbContext.ShoppingLists == null)
            {
                return NotFound();
            }
            ShoppingListId = id.Value;

            var shoppinglistItems = dbContext.ShoppingListItems
                .Include(s => s.SizeUnit)
                .Where(s => s.ShoppingListId == id)
                .OrderByDescending(s => s.Amount).ToArrayAsync();

            ListItems = await shoppinglistItems;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var shoppinglist = await dbContext.ShoppingLists.FindAsync(ShoppingListId);
            if (shoppinglist == null)
            {
                return Page();
            }


            var originalItems = dbContext.ShoppingListItems
                .Where(s => s.ShoppingListId == ShoppingListId);
            foreach (var item in originalItems)
            {
                var newItem = ListItems.Single(i => i.Id == item.Id);
                if (item.Amount != newItem.Amount)
                {
                    item.Amount = newItem.Amount;
                    dbContext.Entry(item).State = EntityState.Modified;
                }
            }
            await dbContext.SaveChangesAsync();


            var routeValues = new
            {
                id = ShoppingListId
            };
            return RedirectToPage("./EditList", routeValues);
        }
    }
}

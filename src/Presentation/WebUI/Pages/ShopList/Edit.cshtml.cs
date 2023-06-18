using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using WebUI.Base;
using Microsoft.Extensions.Logging;
using System;

namespace WebUI.Pages.ShopList
{
    public class EditModel : AuthenticatedPageModelBase<EditModel>
    {
        public EditModel(ILogger<EditModel> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }

        [BindProperty]
        public ShoppingList ShoppingList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || dbContext.ShoppingLists == null)
            {
                return NotFound();
            }

            var shoppinglist = await dbContext.ShoppingLists.FirstOrDefaultAsync(m => m.Id == id);
            if (shoppinglist == null)
            {
                return NotFound();
            }
            ShoppingList = shoppinglist;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var shoppinglist = await dbContext.ShoppingLists.FindAsync(ShoppingList.Id);
            if (shoppinglist == null)
            {
                return Page();
            }

            shoppinglist.Name = ShoppingList.Name;
            shoppinglist.Description = ShoppingList.Description;
            shoppinglist.UpdateUserId = UserId;
            shoppinglist.UpdateTime = DateTime.UtcNow;

            dbContext.Attach(shoppinglist).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingListExists(shoppinglist.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ShoppingListExists(int id)
        {
            return dbContext.ShoppingLists.Any(e => e.Id == id);
        }
    }
}

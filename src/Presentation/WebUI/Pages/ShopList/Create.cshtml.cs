using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeInv.Persistence.Dbo;
using WebUI.Base;
using Microsoft.Extensions.Logging;
using HomeInv.Persistence;
using System;

namespace WebUI.Pages.ShopList
{
    public class CreateModel : AuthenticatedPageModelBase<CreateModel>
    {
        public CreateModel(ILogger<CreateModel> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }

        public IActionResult OnGet()
        {
            //ViewData["HomeId"] = new SelectList(_context.Homes, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public ShoppingList ShoppingList { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ShoppingList.HomeId = UserSettings.DefaultHomeId;
            ShoppingList.IsActive = true;
            ShoppingList.InsertUserId = UserId;
            ShoppingList.InsertTime = DateTime.UtcNow;

            dbContext.ShoppingLists.Add(ShoppingList);
            await dbContext.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

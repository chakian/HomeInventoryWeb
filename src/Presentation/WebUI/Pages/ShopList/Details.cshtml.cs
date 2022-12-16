using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using WebUI.Base;
using Microsoft.Extensions.Logging;

namespace WebUI.Pages.ShopList
{
    public class DetailsModel : AuthenticatedPageModelBase<DetailsModel>
    {
        public DetailsModel(ILogger<DetailsModel> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }

        public ShoppingList ShoppingList { get; set; }

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
            else
            {
                ShoppingList = shoppinglist;
            }
            return Page();
        }
    }
}

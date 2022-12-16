using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using WebUI.Base;
using Microsoft.Extensions.Logging;

namespace WebUI.Pages.ShopList
{
    public class IndexModel : AuthenticatedPageModelBase<IndexModel>
    {
        public IndexModel(ILogger<IndexModel> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }

        public IList<ShoppingList> ShoppingList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (dbContext.ShoppingLists != null)
            {
                ShoppingList = await dbContext.ShoppingLists
                    .Include(s => s.Items)
                    .Include(s => s.InsertUser)
                    .Include(s => s.UpdateUser).ToListAsync();
            }
        }
    }
}

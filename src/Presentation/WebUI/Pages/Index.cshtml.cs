using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using Microsoft.Extensions.Logging;
using WebUI.Base;

namespace WebUI.Pages
{
    public class IndexModel : BasePageModel<IndexModel>
    {
        public IndexModel(ILogger<IndexModel> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }

        public void OnGet()
        {

        }
    }
}

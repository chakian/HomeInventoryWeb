using HomeInv.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;
using WebUI.Base;

namespace WebUI.Pages
{
    public class IndexModel : BasePageModel<IndexModel>
    {
        public IndexModel(ILogger<IndexModel> logger, IHomeService homeService) : base(logger, homeService)
        {
        }

        public void OnGet()
        {

        }
    }
}

using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Base;

namespace WebUI.Pages
{
    public class IndexModel : BasePageModel<IndexModel>
    {
        public IndexModel(ILogger<IndexModel> logger, HomeInventoryDbContext dbContext, IHomeService homeService) : base(logger, dbContext, homeService)
        {
        }

        public void OnGet()
        {

        }
    }
}

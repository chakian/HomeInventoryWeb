using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace WebUI.Base
{
    public class BasePageModel<T> : PageModel
    {
        protected readonly ILogger<T> logger;
        protected readonly HomeInventoryDbContext dbContext;
        protected readonly IHomeService homeService;

        public BasePageModel(ILogger<T> logger, HomeInventoryDbContext dbContext, IHomeService homeService)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.homeService = homeService;
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if(User != null && User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var homes = homeService.GetHomesOfUser(userId);
                string homePath = "/Home/Create";
                if ((homes == null || homes.Count == 0) && context.ActionDescriptor.ViewEnginePath != homePath)
                {
                    context.Result = RedirectToPage(homePath);
                }
            }
            base.OnPageHandlerExecuting(context);
        }
    }
}

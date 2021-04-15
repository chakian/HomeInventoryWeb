using HomeInv.Business;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace WebUI.Base
{
    public abstract class BasePageModel<T> : PageModel
    {
        private readonly HomeInventoryDbContext dbContext;
        protected readonly ILogger<T> logger;

        public BasePageModel(ILogger<T> logger, HomeInventoryDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        protected string UserId
        {
            get
            {
                string id = "";
                if (User != null && User.Identity.IsAuthenticated)
                {
                    var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                    {
                        id = claim.Value;
                    }
                }
                return id;
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                dbContext.Database.BeginTransaction();
                var result = OnModelPost();
                dbContext.Database.CommitTransaction();

                return result;
            }
            catch
            {
                dbContext.Database.RollbackTransaction();

                return Page();
            }
        }

        protected virtual IActionResult OnModelPost()
        {
            throw new NotImplementedException();
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if(User != null && User.Identity.IsAuthenticated)
            {
                IHomeService homeService = new HomeService(dbContext);
                GetHomesOfUserRequest request = new GetHomesOfUserRequest { UserId = UserId };
                var homesResponse = homeService.GetHomesOfUser(request);
                string homePath = "/Home/Create";
                if (homesResponse.Homes.Count == 0 && context.ActionDescriptor.ViewEnginePath != homePath)
                {
                    context.Result = RedirectToPage(homePath);
                }
            }
            base.OnPageHandlerExecuting(context);
        }
    }
}

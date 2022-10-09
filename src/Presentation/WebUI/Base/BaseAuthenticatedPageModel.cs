using HomeInv.Business;
using HomeInv.Common.Constants;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace WebUI.Base
{
    [Authorize]
    public class BaseAuthenticatedPageModel<T> : BasePageModel<T>
    {
        private readonly HomeInventoryDbContext dbContext;
        protected int SelectedHomeId { get; private set; }
        public BaseAuthenticatedPageModel(ILogger<T> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
            this.dbContext = dbContext;
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                var homeService = new HomeService(dbContext);
                GetHomesOfUserRequest request = new GetHomesOfUserRequest { RequestUserId = UserId };
                var homesResponse = homeService.GetHomesOfUser(request);
                string homeCreationPath = "/Home/Create";
                if (homesResponse.Homes.Count == 0 && context.ActionDescriptor.ViewEnginePath != homeCreationPath)
                {
                    context.Result = RedirectToPage(homeCreationPath);
                }
                else
                {
                    if (!context.HttpContext.Session.Keys.Contains(SessionKeys.ACTIVE_HOME_ID))
                    {
                        //TODO: Find currently active home and write to session

                        SelectedHomeId = homesResponse.Homes.FirstOrDefault()?.Id ?? 0;

                        if (SelectedHomeId != 0) context.HttpContext.Session.SetInt32(SessionKeys.ACTIVE_HOME_ID, SelectedHomeId);
                    }
                    else
                    {
                        SelectedHomeId = HttpContext.Session.GetInt32(SessionKeys.ACTIVE_HOME_ID) ?? 0;
                    }
                }
            }
            base.OnPageHandlerExecuting(context);
        }
    }
}

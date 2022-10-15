using HomeInv.Business;
using HomeInv.Common.Constants;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Language;
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
                string currentPath = context.ActionDescriptor.ViewEnginePath;
                string homeCreationPath = "/Home/Create";
                string[] allowedPathsWithoutChecks = new string[] { homeCreationPath };

                if (!allowedPathsWithoutChecks.Contains(currentPath))
                {
                    if((context.HttpContext.Session.GetInt32(SessionKeys.ACTIVE_HOME_ID) ?? 0) == 0)
                    {
                        // find active home id, redirect if it doesn't exist
                        var isHomeFound = SetActiveHomeId(context);
                        if (!isHomeFound)
                        {
                            SetInfoMessage(Resources.Warning_HomeNeededToUseTheApp);
                            context.Result = RedirectToPage(homeCreationPath);
                        }
                    }
                }
            }
            else
            {
                SetErrorMessage("Kapattık kardeşim. Giriş yapıp tekrar bir deneyin.");
                context.Result = RedirectToPage("/");
            }

            // Set site-wide used variables
            SelectedHomeId = context.HttpContext.Session.GetInt32(SessionKeys.ACTIVE_HOME_ID) ?? 0;

            base.OnPageHandlerExecuting(context);
        }

        private bool SetActiveHomeId(PageHandlerExecutingContext context)
        {
            var homeService = new HomeService(dbContext);
            GetHomesOfUserRequest request = new GetHomesOfUserRequest { RequestUserId = UserId };
            var homesResponse = homeService.GetHomesOfUser(request);

            if(homesResponse != null && homesResponse.Homes != null && homesResponse.Homes.Count != 0)
            {
                var homeId = homesResponse.Homes.FirstOrDefault().Id;
                context.HttpContext.Session.SetInt32(SessionKeys.ACTIVE_HOME_ID, homeId);

                return true;
            }

            return false;
        }
    }
}

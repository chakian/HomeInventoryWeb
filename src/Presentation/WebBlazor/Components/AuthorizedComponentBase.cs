using HomeInv.Common.Entities;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace WebBlazor.Components
{
    public class AuthorizedComponentBase : ComponentBase
    {
        protected UserSettingEntity UserSettings { get; private set; }
        protected DialogOptions dialogOptions = new DialogOptions() { MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };

        protected string GetPathOfImageInHome(string fileName, int homeId)
        {
            return $"/uploads/{homeId}/{fileName}";
        }

        protected void GetUserSettings(AuthenticationStateProvider authenticationStateProvider, SignInManager<User> signInManager, UserManager<User> userManager, HomeInventoryDbContext dbContext)
        {
            string userId = "";
            var authenticationState = authenticationStateProvider.GetAuthenticationStateAsync().Result;
            if (signInManager.IsSignedIn(authenticationState.User))
            {
                userId = userManager.GetUserId(authenticationState.User) ?? "";
                if (!string.IsNullOrEmpty(userId))
                {
                    var settingDbo = dbContext.UserSettings.SingleOrDefault(setting => setting.UserId == userId);
                    if (settingDbo != null)
                    {
                        UserSettings = new UserSettingEntity()
                        {
                            UserId = userId,
                            DefaultHomeId = settingDbo.DefaultHomeId
                        };
                    }
                    else
                    {
                        // If the user does not have a settings record that means they haven't created a Home by themselves.
                        // check if they are part of a home
                        var userHomes = dbContext.HomeUsers.Where(hu => hu.UserId == userId).ToList();
                        if (userHomes != null && userHomes.Any())
                        {
                            var _settingDbo = new UserSetting()
                            {
                                UserId = userId,
                                DefaultHomeId = userHomes.First().HomeId,
                                IsActive = true,
                                InsertUserId = userId,
                                InsertTime = DateTime.UtcNow
                            };
                            dbContext.UserSettings.Add(_settingDbo);
                            dbContext.SaveChanges();

                            UserSettings = new UserSettingEntity()
                            {
                                UserId = userId,
                                DefaultHomeId = userHomes.First().HomeId
                            };
                        }
                        else
                        {
                            UserSettings = new UserSettingEntity() { };
                            WarnAndRedirectToHomeCreation();
                        }
                    }
                }
            }
        }

        private void WarnAndRedirectToHomeCreation()
        {
            //string currentPath = context.ActionDescriptor.ViewEnginePath;
            string homeCreationPath = "/Home/Create";

            //SetInfoMessage(Resources.Warning_HomeNeededToUseTheApp);
            //if (currentPath != homeCreationPath) context.Result = RedirectToPage(homeCreationPath);
        }
    }
}

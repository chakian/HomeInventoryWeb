using HomeInv.Common.Entities;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace WebBlazor.Components
{
    public class AuthorizedComponentBase : ComponentBase
    {
        protected UserSettingEntity UserSettings { get; private set; } = default!;
        protected DialogOptions dialogOptions = new()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            DisableBackdropClick = true,
            CloseOnEscapeKey = true
        };

        protected string GetPathOfImageInHome(string fileName, int homeId)
        {
            return $"/uploads/{homeId}/{fileName}";
        }

        protected async Task GetUserSettings(AuthenticationStateProvider authenticationStateProvider, 
            SignInManager<User> signInManager, 
            UserManager<User> userManager, 
            HomeInventoryDbContext dbContext,
            NavigationManager navigationManager)
        {
            string userId = "";
            var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
            if (signInManager.IsSignedIn(authenticationState.User))
            {
                userId = userManager.GetUserId(authenticationState.User) ?? "";
                if (!string.IsNullOrEmpty(userId))
                {
                    var settingDbo = await dbContext.UserSettings.SingleOrDefaultAsync(setting => setting.UserId == userId);
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
                        var userHomes = await dbContext.HomeUsers.Where(hu => hu.UserId == userId).ToListAsync();
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
                            await dbContext.UserSettings.AddAsync(_settingDbo);
                            await dbContext.SaveChangesAsync();

                            UserSettings = new UserSettingEntity()
                            {
                                UserId = userId,
                                DefaultHomeId = userHomes.First().HomeId
                            };
                        }
                        else
                        {
                            UserSettings = new UserSettingEntity() { };
                            WarnAndRedirectToHomeCreation(navigationManager);
                        }
                    }
                }
            }
        }

        private static void WarnAndRedirectToHomeCreation(NavigationManager navigationManager)
        {
            string homeCreationPath = "/homes";
            navigationManager.NavigateTo(homeCreationPath);
        }
    }
}

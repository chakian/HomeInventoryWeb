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
        [Inject] public HomeInventoryDbContext DbContext { get; set; } = default!;
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject] public SignInManager<User> SignInManager { get; set; } = default!;
        [Inject] public UserManager<User> UserManager { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        protected int DefaultAreaId { get; private set; }
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

        protected async Task GetUserSettingsAsync()
        {
            string userId = "";
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (SignInManager.IsSignedIn(authenticationState.User))
            {
                var user = await UserManager.GetUserAsync(authenticationState.User);
                userId = await UserManager.GetUserIdAsync(user) ?? "";
                if (!string.IsNullOrEmpty(userId))
                {
                    var settingDbo = await DbContext.UserSettings.SingleOrDefaultAsync(setting => setting.UserId == userId);
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
                        var userHomes = await DbContext.HomeUsers.Where(hu => hu.UserId == userId).ToListAsync();
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
                            await DbContext.UserSettings.AddAsync(_settingDbo);
                            await DbContext.SaveChangesAsync();

                            UserSettings = new UserSettingEntity()
                            {
                                UserId = userId,
                                DefaultHomeId = userHomes.First().HomeId
                            };
                        }
                        else
                        {
                            UserSettings = new UserSettingEntity() { };
                            await RedirectToHomeCreationAsync();
                            return;
                        }
                    }

                    int? _areaId = (await DbContext.Areas.FirstOrDefaultAsync(a => a.HomeId == UserSettings.DefaultHomeId))?.Id;
                    if (_areaId == null)
                    {
                        Area defaultArea = new()
                        {
                            HomeId = UserSettings.DefaultHomeId,
                            InsertTime = DateTime.UtcNow,
                            InsertUserId = userId,
                            IsActive = true,
                            Name = "Genel"
                        };
                        await DbContext.Areas.AddAsync(defaultArea);
                        await DbContext.SaveChangesAsync();
                        DefaultAreaId = defaultArea.Id;
                    }
                    else
                    {
                        DefaultAreaId = _areaId.Value;
                    }
                }
            }
        }

        private async Task RedirectToHomeCreationAsync()
        {
            if (!NavigationManager.Uri.Contains("/homes"))
            {
                string homeCreationPath = "/homes";
                NavigationManager.NavigateTo(homeCreationPath);
            }
            await Task.Yield();
        }
    }
}

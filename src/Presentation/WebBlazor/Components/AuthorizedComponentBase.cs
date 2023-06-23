using HomeInv.Business.Services;
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

        protected void GetUserSettings()
        {
            string userId = "";
            var authenticationState = AuthenticationStateProvider.GetAuthenticationStateAsync().Result;
            if (SignInManager.IsSignedIn(authenticationState.User))
            {
                userId = UserManager.GetUserId(authenticationState.User) ?? "";
                if (!string.IsNullOrEmpty(userId))
                {
                    var settingDbo = DbContext.UserSettings.SingleOrDefault(setting => setting.UserId == userId);
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
                        var userHomes = DbContext.HomeUsers.Where(hu => hu.UserId == userId).ToList();
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
                            DbContext.UserSettings.Add(_settingDbo);
                            DbContext.SaveChanges();

                            UserSettings = new UserSettingEntity()
                            {
                                UserId = userId,
                                DefaultHomeId = userHomes.First().HomeId
                            };
                        }
                        else
                        {
                            UserSettings = new UserSettingEntity() { };
                            RedirectToHomeCreation();
                            return;
                        }
                    }

                    int? _areaId = DbContext.Areas.FirstOrDefault(a => a.HomeId == UserSettings.DefaultHomeId)?.Id;
                    if (_areaId == null)
                    {
                        Area defaultArea = new()
                        {
                            HomeId = UserSettings.DefaultHomeId,
                            InsertTime = DateTime.UtcNow,
                            InsertUserId= userId,
                            IsActive= true,
                            Name="Genel"
                        };
                        DbContext.Areas.Add(defaultArea);
                        DbContext.SaveChanges();
                        DefaultAreaId = defaultArea.Id;
                    }
                    else
                    {
                        DefaultAreaId = _areaId.Value;
                    }
                }
            }
        }

        private void RedirectToHomeCreation()
        {
            string homeCreationPath = "/homes";
            NavigationManager.NavigateTo(homeCreationPath);
        }
    }
}

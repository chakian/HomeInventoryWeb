using HomeInv.Common.Entities;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;

namespace WebUI.Base
{
    [Authorize]
    public class BaseAuthenticatedPageModel<T> : BasePageModel<T>
    {
        private readonly HomeInventoryDbContext dbContext;
        protected UserSettingEntity UserSettings { get; private set; }

        public BaseAuthenticatedPageModel(ILogger<T> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
            this.dbContext = dbContext;
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                var settings = ReadUserSettingsFromSession(context);
                if(settings != null)
                {
                    UserSettings = settings;
                }
                else
                {
                    var settingDbo = dbContext.UserSettings.SingleOrDefault(setting => setting.UserId == UserId);
                    if (settingDbo != null) {
                        UserSettings = new UserSettingEntity()
                        {
                            UserId = UserId,
                            DefaultHomeId = settingDbo.DefaultHomeId
                        };
                        WriteUserSettingsToSession(context, UserSettings);
                    }
                    else
                    {
                        // If the user does not have a settings record that means they haven't created a Home by themselves.
                        // check if they are part of a home
                        var userHomes = dbContext.HomeUsers.Where(hu => hu.UserId == UserId).ToList();
                        if (userHomes != null && userHomes.Any())
                        {
                            var _settingDbo = new UserSetting()
                            {
                                UserId = UserId,
                                DefaultHomeId = userHomes.First().HomeId,
                                IsActive = true,
                                InsertUserId = UserId,
                                InsertTime = DateTime.UtcNow
                            };
                            dbContext.UserSettings.Add(_settingDbo);
                            dbContext.SaveChanges();

                            UserSettings = new UserSettingEntity()
                            {
                                UserId = UserId,
                                DefaultHomeId = userHomes.First().HomeId
                            };
                            WriteUserSettingsToSession(context, UserSettings);
                        }
                        else
                        {
                            WarnAndRedirectToHomeCreation(context);
                        }
                    }
                }
            }
            else
            {
                SetErrorMessage(Resources.Error_LoggedOut);
                context.Result = RedirectToPage("/");
            }

            base.OnPageHandlerExecuting(context);
        }

        private void WarnAndRedirectToHomeCreation(PageHandlerExecutingContext context)
        {
            string currentPath = context.ActionDescriptor.ViewEnginePath;
            string homeCreationPath = "/Home/Create";

            SetInfoMessage(Resources.Warning_HomeNeededToUseTheApp);
            if (currentPath != homeCreationPath) context.Result = RedirectToPage(homeCreationPath);
        }

        private UserSettingEntity ReadUserSettingsFromSession(PageHandlerExecutingContext context)
        {
            var settingsJson = context.HttpContext.Session.Get("UserSettings");
            if(settingsJson != null)
            {
                var settings = JsonSerializer.Deserialize<UserSettingEntity>(settingsJson);
                if(settings != null && settings.UserId == UserId) return settings;
                else context.HttpContext.Session.Clear();
            }
            else context.HttpContext.Session.Clear();
            return null;
        }
        private void WriteUserSettingsToSession(PageHandlerExecutingContext context, UserSettingEntity userSettings)
        {
            var settingsJson = JsonSerializer.SerializeToUtf8Bytes(userSettings);
            context.HttpContext.Session.Set("UserSettings", settingsJson);
        }
    }
}

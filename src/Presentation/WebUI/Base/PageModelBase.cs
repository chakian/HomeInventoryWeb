using HomeInv.Common.Entities;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace WebUI.Base
{
    public class PageModelBase<T> : PageModel
    {
        protected readonly HomeInventoryDbContext dbContext;
        protected readonly ILogger<T> logger;
        private const string DEFAULT_LANDING_PAGE_FOR_LOGGEDIN = "/Stock/Overview";

        public PageModelBase(ILogger<T> logger, HomeInventoryDbContext dbContext)
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

        protected void SetErrorMessage(string message)
        {
            var key = "Error";
            AddOrUpdateTempData(key, message);
        }
        protected void SetSuccessMessage(string message)
        {
            var key = "Success";
            AddOrUpdateTempData(key, message);
        }
        protected void SetInfoMessage(string message)
        {
            var key = "Info";
            AddOrUpdateTempData(key, message);
        }
        protected void SetWarningMessage(string message)
        {
            var key = "Warning";
            AddOrUpdateTempData(key, message);
        }
        private void AddOrUpdateTempData(string key, string message)
        {
            var _tempData = TempData.Peek(key);
            string currentText = (_tempData != null) ? _tempData.ToString() : "";
            if (string.IsNullOrEmpty(currentText.ToString()))
            {
                TempData.Add(key, message);
            }
            else if (!currentText.Contains(message))
            {
                TempData.Remove(key);
                currentText += " " + message;
                TempData.Add(key, currentText);
            }
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (User != null && User.Identity.IsAuthenticated && (context.ActionDescriptor.ViewEnginePath == "/" || context.ActionDescriptor.ViewEnginePath == "/Index"))
            {
                context.Result = RedirectToPage(DEFAULT_LANDING_PAGE_FOR_LOGGEDIN);
            }
            base.OnPageHandlerExecuting(context);
        }
    }

    [Authorize]
    public class AuthenticatedPageModelBase<T> : PageModelBase<T>
    {
        protected UserSettingEntity UserSettings { get; private set; }

        public AuthenticatedPageModelBase(ILogger<T> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr-TR");
            if (User != null && User.Identity.IsAuthenticated)
            {
                var settings = ReadUserSettingsFromSession(context);
                if (settings != null)
                {
                    UserSettings = settings;
                }
                else
                {
                    var settingDbo = dbContext.UserSettings.SingleOrDefault(setting => setting.UserId == UserId);
                    if (settingDbo != null)
                    {
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
                context.HttpContext.Session.Clear();
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

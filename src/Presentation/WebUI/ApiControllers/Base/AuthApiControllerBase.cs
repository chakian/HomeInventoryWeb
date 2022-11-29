using HomeInv.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Linq;

namespace WebUI.ApiControllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiControllerBase : Controller
    {
        protected string UserId { get; private set; }
        protected int DefaultHomeId { get; private set; }

        readonly IUserSettingService _userSettingService;

        public AuthApiControllerBase(IUserSettingService userSettingService)
        {
            _userSettingService = userSettingService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User != null && User.HasClaim(c => c.Type == "Id"))
            {
                var idClaim = User.Claims.SingleOrDefault(c => c.Type == "Id");
                if (idClaim != null)
                {
                    UserId = idClaim.Value;

                    var settings = _userSettingService.GetUserSettings(new HomeInv.Common.ServiceContracts.UserSetting.GetUserSettingsRequest()
                    {
                        UserId = UserId,
                        RequestUserId = UserId
                    });
                    if (settings != null && settings.UserSetting != null && settings.UserSetting.DefaultHomeId > 0)
                    {
                        DefaultHomeId = settings.UserSetting.DefaultHomeId;
                    }
                }
            }

            base.OnActionExecuting(context);
        }

        public override NotFoundObjectResult NotFound([ActionResultObjectValue] object value)
        {
            return base.NotFound(new { message = value.ToString() });
        }
    }
}

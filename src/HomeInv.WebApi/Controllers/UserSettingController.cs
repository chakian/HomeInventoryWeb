using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.WebApi.Contracts.UserSetting;
using Microsoft.AspNetCore.Mvc;

namespace HomeInv.WebApi.Controllers;

[ApiController]
[Route("user-setting")]
public class UserSettingController : BaseController
{
    private readonly IUserSettingService _userSettingService;

    public UserSettingController(IUserSettingService userSettingService)
    {
        _userSettingService = userSettingService;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync(
        UpdateUserSettingsRequest request,
        CancellationToken ct)
    {
        var updateUserSettingsRequest = new Common.ServiceContracts.UserSetting.UpdateUserSettingsRequest()
        {
            UserSettingEntity = new UserSettingEntity()
            {
                DefaultHomeId = request.DefaultHomeId,
                UserId = request.UserId
            },
            RequestUserId = request.UserId
        };
        var serviceResponse = await _userSettingService.UpdateUserSettingsAsync(updateUserSettingsRequest, ct);
        return CheckErrorForOkResult(serviceResponse);
    }
}

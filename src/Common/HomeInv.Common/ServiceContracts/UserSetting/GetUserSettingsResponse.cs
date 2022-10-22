using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.UserSetting
{
    public class GetUserSettingsResponse : BaseResponse
    {
        public UserSettingEntity UserSetting { get; set; }
    }
}

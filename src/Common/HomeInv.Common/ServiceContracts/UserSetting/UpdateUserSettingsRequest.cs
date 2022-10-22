using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.UserSetting
{
    public class UpdateUserSettingsRequest : BaseRequest
    {
        public UserSettingEntity UserSettingEntity { get; set; }
    }
}

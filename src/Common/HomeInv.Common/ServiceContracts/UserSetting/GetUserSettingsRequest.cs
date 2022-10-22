namespace HomeInv.Common.ServiceContracts.UserSetting
{
    public class GetUserSettingsRequest : BaseRequest
    {
        public string UserId { get; set; }
    }
}

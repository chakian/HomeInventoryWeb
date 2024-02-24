namespace HomeInv.WebApi.Contracts.UserSetting;

public class UpdateUserSettingsRequest : BaseRequest
{
    public int DefaultHomeId { get; set; }
}

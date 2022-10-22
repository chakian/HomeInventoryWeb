using HomeInv.Common.ServiceContracts.UserSetting;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IUserSettingService<D> : IUserSettingService, IServiceBase<D, Entities.UserSettingEntity>
        where D : class
    {
    }

    public interface IUserSettingService : IServiceBase
    {
        InsertOrUpdateForDefaultHomeResponse InsertOrUpdateForDefaultHome(InsertOrUpdateForDefaultHomeRequest request);
        GetUserSettingsResponse GetUserSettings(GetUserSettingsRequest request);
        UpdateUserSettingsResponse UpdateUserSettings(UpdateUserSettingsRequest request);
    }
}

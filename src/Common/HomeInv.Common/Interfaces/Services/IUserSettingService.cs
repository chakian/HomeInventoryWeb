using System.Threading;
using System.Threading.Tasks;
using HomeInv.Common.ServiceContracts.UserSetting;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IUserSettingService<D> : IUserSettingService, IServiceBase<D, Entities.UserSettingEntity>
        where D : class
    {
    }

    public interface IUserSettingService : IServiceBase
    {
        Task<InsertOrUpdateForDefaultHomeResponse> InsertOrUpdateForDefaultHomeAsync(InsertOrUpdateForDefaultHomeRequest request, CancellationToken ct);
        Task<GetUserSettingsResponse> GetUserSettingsAsync(GetUserSettingsRequest request, CancellationToken ct);
        Task<UpdateUserSettingsResponse> UpdateUserSettingsAsync(UpdateUserSettingsRequest request, CancellationToken ct);
    }
}

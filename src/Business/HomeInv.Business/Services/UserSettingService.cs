using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.UserSetting;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeInv.Business.Services
{
    public class UserSettingService : AuditableServiceBase<UserSetting, UserSettingEntity>, IUserSettingService<UserSetting>
    {
        public UserSettingService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override UserSettingEntity ConvertDboToEntity(UserSetting dbo)
        {
            return new UserSettingEntity()
            {
                Id = dbo.Id,
                UserId = dbo.UserId,
                DefaultHomeId = dbo.DefaultHomeId,
                DefaultHome = new HomeEntity()
                {
                    Id = dbo.DefaultHome.Id,
                    Name = dbo.DefaultHome.Name,
                    Description = dbo.DefaultHome.Description
                }
            };
        }

        public async Task<GetUserSettingsResponse> GetUserSettingsAsync(GetUserSettingsRequest request, CancellationToken ct)
        {
            var response = new GetUserSettingsResponse();

            var dbo = await context.UserSettings.Include(setting => setting.DefaultHome).SingleOrDefaultAsync(setting => setting.UserId == request.UserId, ct);
            if (dbo != null)
            {
                response.UserSetting = ConvertDboToEntity(dbo);
            }
            else
            {
                response.UserSetting = new UserSettingEntity()
                {
                    UserId = request.UserId,
                };
            }

            return response;
        }

        public async Task<InsertOrUpdateForDefaultHomeResponse> InsertOrUpdateForDefaultHomeAsync(InsertOrUpdateForDefaultHomeRequest request, CancellationToken ct)
        {
            var response = new InsertOrUpdateForDefaultHomeResponse();

            var settingDbo = await context.UserSettings.SingleOrDefaultAsync(setting => setting.UserId == request.UserId, ct);
            if (settingDbo != null)
            {
                settingDbo.DefaultHomeId = request.DefaultHomeId;
            }
            else
            {
                settingDbo = CreateNewAuditableObject(request);
                settingDbo.UserId = request.UserId;
                settingDbo.DefaultHomeId = request.DefaultHomeId;
                context.UserSettings.Add(settingDbo);
            }
            await context.SaveChangesAsync(ct);

            return response;
        }

        public async Task<UpdateUserSettingsResponse> UpdateUserSettingsAsync(UpdateUserSettingsRequest request, CancellationToken ct)
        {
            var response = new UpdateUserSettingsResponse();

            var settingDbo = await context.UserSettings.SingleOrDefaultAsync(setting => setting.UserId == request.UserSettingEntity.UserId, ct);
            if (settingDbo != null)
            {
                settingDbo.DefaultHomeId = request.UserSettingEntity.DefaultHomeId;
            }
            else
            {
                settingDbo = CreateNewAuditableObject(request);
                settingDbo.UserId = request.UserSettingEntity.UserId;
                settingDbo.DefaultHomeId = request.UserSettingEntity.DefaultHomeId;
                context.UserSettings.Add(settingDbo);
            }
            await context.SaveChangesAsync(ct);

            return response;
        }
    }
}

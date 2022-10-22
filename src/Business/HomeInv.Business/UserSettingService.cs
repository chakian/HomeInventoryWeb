using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.UserSetting;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HomeInv.Business
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

        public GetUserSettingsResponse GetUserSettings(GetUserSettingsRequest request)
        {
            var response = new GetUserSettingsResponse();

            var dbo = context.UserSettings.Include(setting => setting.DefaultHome).SingleOrDefault(setting => setting.UserId == request.UserId);
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

        public InsertOrUpdateForDefaultHomeResponse InsertOrUpdateForDefaultHome(InsertOrUpdateForDefaultHomeRequest request)
        {
            var response = new InsertOrUpdateForDefaultHomeResponse();

            var settingDbo = context.UserSettings.SingleOrDefault(setting => setting.UserId == request.UserId);
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
            context.SaveChanges();

            return response;
        }

        public UpdateUserSettingsResponse UpdateUserSettings(UpdateUserSettingsRequest request)
        {
            var response = new UpdateUserSettingsResponse();

            var settingDbo = context.UserSettings.SingleOrDefault(setting => setting.UserId == request.UserSettingEntity.UserId);
            if(settingDbo != null)
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
            context.SaveChanges();

            return response;
        }
    }
}

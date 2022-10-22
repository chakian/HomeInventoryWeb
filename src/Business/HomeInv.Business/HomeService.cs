using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Common.ServiceContracts.HomeUser;
using HomeInv.Common.ServiceContracts.UserSetting;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business
{
    public class HomeService : AuditableServiceBase<Home, HomeEntity>, IHomeService<Home>
    {
        public HomeService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        private IQueryable<Home> GetHomesOfUserInternal(string userId)
        {
            var homes = GetAllActiveAsQueryable().Join(context.HomeUsers,
                home => home.Id,
                homeUser => homeUser.HomeId,
                (home, homeUser) => new { Home = home, HomeUser = homeUser })
                .Where(homeAndUser => homeAndUser.HomeUser.UserId == userId && homeAndUser.HomeUser.IsActive);

            return homes.Select(homeAndUser => homeAndUser.Home);
        }

        private Home GetSingleHomeOfUserInternal(string userId, int homeId)
        {
            //TODO: Checking if this user belongs to requested home could be done better
            var homes = GetHomesOfUserInternal(userId);
            var home = homes.SingleOrDefault(home => home.Id == homeId);
            return home;
        }

        public CreateHomeResponse CreateHome(CreateHomeRequest request)
        {
            CreateHomeResponse response = new CreateHomeResponse();
            Home home = CreateNewAuditableObject(request);
            home.Name = request.HomeEntity.Name;
            home.Description = request.HomeEntity.Description;

            context.Homes.Add(home);
            context.SaveChanges();

            var insertHomeUserRequest = new InsertHomeUserRequest()
            {
                HomeId = home.Id,
                UserId = request.RequestUserId,
                Role = "owner",
                RequestUserId = request.RequestUserId,
            };
            var _userService = new HomeUserService(context);
            var insertHomeUserResponse = _userService.InsertHomeUser(insertHomeUserRequest);
            if(insertHomeUserResponse != null && !insertHomeUserResponse.IsSuccessful)
            {
                response.Result.Messages.AddRange(insertHomeUserResponse.Result.Messages);
            }

            var _settingService = new UserSettingService(context);
            var getUserSettingsRequest = new GetUserSettingsRequest()
            {
                UserId = request.RequestUserId,
                RequestUserId = request.RequestUserId
            };
            var getUserSettingsResponse = _settingService.GetUserSettings(getUserSettingsRequest);
            if (getUserSettingsResponse.UserSetting.DefaultHomeId <= 0)
            {
                var insertOrUpdateForDefaultHomeRequest = new InsertOrUpdateForDefaultHomeRequest()
                {
                    UserId = request.RequestUserId,
                    DefaultHomeId = home.Id,
                    RequestUserId = request.RequestUserId
                };

                var insertOrUpdateForDefaultHomeResponse = _settingService.InsertOrUpdateForDefaultHome(insertOrUpdateForDefaultHomeRequest);
                if (insertOrUpdateForDefaultHomeResponse != null && !insertOrUpdateForDefaultHomeResponse.IsSuccessful)
                {
                    response.Result.Messages.AddRange(insertOrUpdateForDefaultHomeResponse.Result.Messages);
                }
            }

            response.HomeEntity = ConvertDboToEntity(home);
            return response;
        }

        public override HomeEntity ConvertDboToEntity(Home dbo)
        {
            return new HomeEntity()
            {
                Id = dbo.Id,
                Name = dbo.Name,
                Description = dbo.Description
            };
        }

        public GetHomesOfUserResponse GetHomesOfUser(GetHomesOfUserRequest request)
        {
            var response = new GetHomesOfUserResponse();
            var homes = GetHomesOfUserInternal(request.RequestUserId);
            List<HomeEntity> homeEntities = new List<HomeEntity>();
            foreach (var home in homes)
            {
                homeEntities.Add(ConvertDboToEntity(home));
            }
            response.Homes = homeEntities;
            return response;
        }

        public GetSingleHomeOfUserResponse GetSingleHomeOfUser(GetSingleHomeOfUserRequest request)
        {
            var response = new GetSingleHomeOfUserResponse();
            var home = GetSingleHomeOfUserInternal(request.RequestUserId, request.HomeId);
            response.Home = ConvertDboToEntity(home);
            return response;
        }

        public UpdateHomeResponse UpdateHome(UpdateHomeRequest request)
        {
            var response = new UpdateHomeResponse();
            Home home = GetSingleHomeOfUserInternal(request.RequestUserId, request.HomeEntity.Id);
            UpdateAuditableObject(home, request.RequestUserId);

            home.Name = request.HomeEntity.Name;
            home.Description = request.HomeEntity.Description;

            context.Entry(home).State = EntityState.Modified;
            context.SaveChanges();

            response.HomeEntity = ConvertDboToEntity(home);
            return response;
        }
    }
}

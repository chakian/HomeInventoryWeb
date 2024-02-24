using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Common.ServiceContracts.HomeUser;
using HomeInv.Common.ServiceContracts.UserSetting;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeInv.Business.Services
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

        private async Task<Home> GetSingleHomeOfUserInternalAsync(string userId, int homeId, CancellationToken ct)
        {
            //TODO: Checking if this user belongs to requested home could be done better
            var homes = GetHomesOfUserInternal(userId);
            var home = await homes.SingleOrDefaultAsync(home => home.Id == homeId, ct);
            return home;
        }

        public async Task<CreateHomeResponse> CreateHomeAsync(CreateHomeRequest request, CancellationToken ct)
        {
            CreateHomeResponse response = new();

            var homesOfUser = GetHomesOfUserInternal(request.RequestUserId);
            if(homesOfUser != null && homesOfUser.Any(home => home.Name.Trim() == request.HomeEntity.Name.Trim()))
            {
                response.AddError(Resources.Home_Error_SameNameExists);
                return response;
            }

            var home = CreateNewAuditableObject(request);
            home.Name = request.HomeEntity.Name;
            home.Description = request.HomeEntity.Description;

            context.Homes.Add(home);
            await context.SaveChangesAsync(ct);

            var insertHomeUserRequest = new InsertHomeUserRequest()
            {
                HomeId = home.Id,
                UserId = request.RequestUserId,
                Role = "owner",
                RequestUserId = request.RequestUserId,
            };
            var userService = new HomeUserService(context);
            var insertHomeUserResponse = await userService.InsertHomeUserAsync(insertHomeUserRequest, ct);
            if (insertHomeUserResponse != null && !insertHomeUserResponse.IsSuccessful)
            {
                response.Result.Messages.AddRange(insertHomeUserResponse.Result.Messages);
            }

            var settingService = new UserSettingService(context);
            var getUserSettingsRequest = new GetUserSettingsRequest()
            {
                UserId = request.RequestUserId,
                RequestUserId = request.RequestUserId
            };
            var getUserSettingsResponse = await settingService.GetUserSettingsAsync(getUserSettingsRequest, ct);
            if (getUserSettingsResponse.UserSetting.DefaultHomeId <= 0)
            {
                var insertOrUpdateForDefaultHomeRequest = new InsertOrUpdateForDefaultHomeRequest()
                {
                    UserId = request.RequestUserId,
                    DefaultHomeId = home.Id,
                    RequestUserId = request.RequestUserId
                };

                var insertOrUpdateForDefaultHomeResponse = await settingService.InsertOrUpdateForDefaultHomeAsync(insertOrUpdateForDefaultHomeRequest, ct);
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

        public async Task<GetHomesOfUserResponse> GetHomesOfUserAsync(GetHomesOfUserRequest request, CancellationToken ct)
        {
            var response = new GetHomesOfUserResponse();
            var homes = await GetHomesOfUserInternal(request.RequestUserId).ToListAsync(ct);
            List<HomeEntity> homeEntities = new();
            foreach (var home in homes)
            {
                homeEntities.Add(ConvertDboToEntity(home));
            }
            response.Homes = homeEntities;
            return response;
        }

        public async Task<UpdateHomeResponse> UpdateHomeAsync(UpdateHomeRequest request, CancellationToken ct)
        {
            var response = new UpdateHomeResponse();
            var home = await GetSingleHomeOfUserInternalAsync(request.RequestUserId, request.HomeEntity.Id, ct);
            UpdateAuditableObject(home, request.RequestUserId);

            home.Name = request.HomeEntity.Name;
            home.Description = request.HomeEntity.Description;

            context.Entry(home).State = EntityState.Modified;
            await context.SaveChangesAsync(ct);

            response.HomeEntity = ConvertDboToEntity(home);
            return response;
        }
    }
}

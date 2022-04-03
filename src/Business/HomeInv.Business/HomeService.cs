using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business
{
    public class HomeService : AuditableServiceBase<Home, HomeEntity>, IHomeService<Home>
    {
        public HomeService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public CreateHomeResponse CreateHome(CreateHomeRequest request)
        {
            CreateHomeResponse response = new CreateHomeResponse();
            Home home = CreateNewAuditableObject(request);
            home.Name = request.HomeEntity.Name;
            home.Description = request.HomeEntity.Description;

            context.Homes.Add(home);
            //context.Entry(home).State = EntityState.Added;
            context.SaveChanges();

            response.HomeEntity = ConvertDboToEntity(home);
            return response;
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
            //TODO: Checking if this user belongs to requested home could be done better
            var homes = GetHomesOfUserInternal(request.RequestUserId);
            var home = homes.SingleOrDefault(home => home.Id == request.HomeId);
            response.Home = ConvertDboToEntity(home);
            return response;
        }
    }
}

using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business
{
    public class HomeService : ServiceBase, IHomeService
    {
        public HomeService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public CreateHomeResponse CreateHome(CreateHomeRequest request)
        {
            CreateHomeResponse response = new CreateHomeResponse();
            Home home = CreateNewAuditableObject<Home>(request);
            home.Name = request.HomeEntity.Name;
            home.Description = request.HomeEntity.Description;
            
            context.Homes.Add(home);
            context.SaveChanges();

            response.HomeEntity = ConvertHomeToHomeEntity(home);
            return response;
        }

        private IQueryable<Home> GetHomesOfUserInternal(string userId)
        {
            var homes = context.Homes.Where(q => q.InsertUserId == userId);
            return homes;
        }

        private HomeEntity ConvertHomeToHomeEntity(Home home)
        {
            return new HomeEntity()
            {
                Id = home.Id,
                Name = home.Name,
                Description = home.Description
            };
        }

        public GetHomesOfUserResponse GetHomesOfUser(GetHomesOfUserRequest request)
        {
            GetHomesOfUserResponse response = new GetHomesOfUserResponse();
            var homes = GetHomesOfUserInternal(request.RequestUserId);
            List<HomeEntity> homeEntities = new List<HomeEntity>();
            foreach (var home in homes)
            {
                homeEntities.Add(ConvertHomeToHomeEntity(home));
            }
            response.Homes = homeEntities;
            return response;
        }
    }
}

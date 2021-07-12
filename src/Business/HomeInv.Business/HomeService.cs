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
            context.SaveChanges();

            response.HomeEntity = ConvertDboToEntity(home);
            return response;
        }

        private IQueryable<Home> GetHomesOfUserInternal(string userId)
        {
            var homes = context.Homes.Where(q => q.InsertUserId == userId);
            return homes;
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
            GetHomesOfUserResponse response = new GetHomesOfUserResponse();
            var homes = GetHomesOfUserInternal(request.RequestUserId);
            List<HomeEntity> homeEntities = new List<HomeEntity>();
            foreach (var home in homes)
            {
                homeEntities.Add(ConvertDboToEntity(home));
            }
            response.Homes = homeEntities;
            return response;
        }
    }
}

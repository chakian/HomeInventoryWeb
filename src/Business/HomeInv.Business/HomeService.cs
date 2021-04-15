using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business
{
    public class HomeService : ServiceBase, IHomeService
    {
        public HomeService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public HomeEntity CreateHome(HomeEntity homeEntity, string userId)
        {
            Home home = CreateNewAuditableObject<Home>(userId);
            home.Name = homeEntity.Name;
            home.Description = homeEntity.Description;
            
            context.Homes.Add(home);
            context.SaveChanges();
            
            homeEntity.Id = home.Id;
            return homeEntity;
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

        public List<HomeEntity> GetHomesOfUser(string userId)
        {
            var homes = GetHomesOfUserInternal(userId);
            List<HomeEntity> homeEntities = new List<HomeEntity>();
            foreach (var home in homes)
            {
                homeEntities.Add(ConvertHomeToHomeEntity(home));
            }
            return homeEntities;
        }
    }
}

using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;

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
    }
}

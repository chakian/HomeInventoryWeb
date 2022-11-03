using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeInv.Business.Services.Tests
{
    public abstract class HomeRelatedTestBase : TestBase
    {
        public HomeRelatedTestBase()
        {
        }

        protected int CreateDefaultHome(HomeInventoryDbContext context)
        {
            var home = new Home()
            {
                Name = "new home"
            };
            context.Homes.Add(home);
            context.SaveChanges();
            
            return home.Id;
        }
    }
}

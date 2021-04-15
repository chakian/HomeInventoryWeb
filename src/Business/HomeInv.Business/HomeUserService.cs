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
    public class HomeUserService : ServiceBase, IHomeUserService
    {
        public HomeUserService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public bool InsertHomeUser(int homeId, string userId, string role)
        {
            if(context.HomeUsers.Any(q=>q.HomeId==homeId && q.UserId == userId))
            {
                return false;
            }

            HomeUser homeUser = CreateNewAuditableObject<HomeUser>(userId);
            homeUser.HomeId = homeId;
            homeUser.UserId = userId;
            homeUser.Role = role;

            context.HomeUsers.Add(homeUser);
            context.SaveChanges();

            return true;
        }
    }
}

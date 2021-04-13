﻿using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeInv.Persistence
{
    public class HomeInventoryDbContext : IdentityDbContext<User>
    {
        public HomeInventoryDbContext(DbContextOptions<HomeInventoryDbContext> options)
            : base(options)
        {
        }
    }
}

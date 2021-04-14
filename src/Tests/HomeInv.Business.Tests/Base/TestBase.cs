using HomeInv.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HomeInv.Business.Tests
{
    public abstract class TestBase
    {
        private readonly DbContextOptions<HomeInventoryDbContext> dbContextOptions;
        protected List<string> userIds = new List<string>();

        public TestBase()
        {
            dbContextOptions = CreateNewContextOptions();
            SeedUserData();
        }

        protected abstract void SeedData();

        private void SeedUserData()
        {
            var context = GetContext();
            string userId = Guid.NewGuid().ToString();
            userIds.Add(userId);
            context.Users.Add(new Persistence.Dbo.User()
            {
                Id = userId,
                UserName = "test1@ab.cd",
                Email = "test1@ab.cd",
                PasswordHash = Guid.NewGuid().ToString()
            });
            userId = Guid.NewGuid().ToString();
            userIds.Add(userId);
            context.Users.Add(new Persistence.Dbo.User()
            {
                Id = userId,
                UserName = "test2@ab.cd",
                Email = "test1@ab.cd",
                PasswordHash = Guid.NewGuid().ToString()
            });
            userId = Guid.NewGuid().ToString();
            userIds.Add(userId);
            context.Users.Add(new Persistence.Dbo.User()
            {
                Id = userId,
                UserName = "test3@ab.cd",
                Email = "test1@ab.cd",
                PasswordHash = Guid.NewGuid().ToString()
            });
            context.SaveChanges();

            SeedData();
        }

        private DbContextOptions<HomeInventoryDbContext> CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<HomeInventoryDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        //protected HomeInventoryDbContext CreateNewContext()
        //{
        //    DbContextOptions<HomeInventoryDbContext> options = CreateNewContextOptions();
        //    return new HomeInventoryDbContext(options);
        //}

        protected HomeInventoryDbContext GetContext()
        {
            return new HomeInventoryDbContext(dbContextOptions);
        }
    }
}

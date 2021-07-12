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
            #region Prepare Users
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
            #endregion

            #region Prepare Homes
            var homeId1 = context.Homes.Add(new Persistence.Dbo.Home()
            {
                Name = "seed 1",
                Description = "seed desc 1",
                //IsActive = true
            }).Entity.Id;
            var homeId2 = context.Homes.Add(new Persistence.Dbo.Home()
            {
                Name = "seed 2",
                Description = "seed desc 2",
                //IsActive = true
            }).Entity.Id;
            var homeId3 = context.Homes.Add(new Persistence.Dbo.Home()
            {
                Name = "seed 3",
                Description = "seed desc 3",
                //IsActive = true
            }).Entity.Id;
            #endregion

            #region Prepare Home Users
            context.HomeUsers.Add(new Persistence.Dbo.HomeUser()
            {
                HomeId = homeId1,
                UserId = userIds[0],
                Role = "owner",
                //IsActive = true
            });
            context.HomeUsers.Add(new Persistence.Dbo.HomeUser()
            {
                HomeId = homeId2,
                UserId = userIds[1],
                Role = "owner",
                //IsActive = true
            });
            context.HomeUsers.Add(new Persistence.Dbo.HomeUser()
            {
                HomeId = homeId3,
                UserId = userIds[2],
                Role = "owner",
                //IsActive = true
            });
            #endregion

            #region Prepare Areas
            //context.Areas.Add(new Persistence.Dbo.Area()
            //{
            //    HomeId = homeId1,
            //    Name = "Area1",
            //    IsActive = true
            //});
            //context.Areas.Add(new Persistence.Dbo.Area()
            //{
            //    HomeId = homeId2,
            //    Name = "Area2",
            //    IsActive = true
            //});
            //context.Areas.Add(new Persistence.Dbo.Area()
            //{
            //    HomeId = homeId3,
            //    Name = "Area3",
            //    IsActive = true
            //});
            #endregion

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

using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using System;
using System.Linq;
using Xunit;

namespace HomeInv.Business.Services.Tests
{
    public class HomeTests : TestBase, IDisposable
    {
        public void Dispose()
        {
        }

        protected override void SeedData()
        {
        }

        [Fact]
        public void Home_Create_Ok()
        {
            // arrange
            var context = GetContext();
            IHomeService homeService = new HomeService(context);
            HomeEntity homeEntity = new HomeEntity()
            {
                Name = "test home 1",
                Description = "test desc 1"
            };

            // act
            var request = new CreateHomeRequest() { HomeEntity = homeEntity, RequestUserId = userIds[0] };
            var actual = homeService.CreateHome(request);
            var expected = context.Homes.Find(4);
            string actualInsertUserId = expected.InsertUserId;

            // assert
            Assert.Equal(expected.Name, actual.HomeEntity.Name);
            Assert.Equal(userIds[0], actualInsertUserId);
        }

        [Fact]
        public void Home_GetHomesOfUser_Ok()
        {
            // arrange
            var context = GetContext();
            IHomeService homeService = new HomeService(context);
            var homeId = context.Homes.Add(new Persistence.Dbo.Home()
            {
                Name = "test home 1",
                Description = "test desc 1",
                IsActive = true
            }).Entity.Id;
            context.HomeUsers.Add(new Persistence.Dbo.HomeUser()
            {
                HomeId = homeId,
                UserId = userIds[0],
                Role = "owner",
                IsActive = true
            });
            context.SaveChanges();
            
            // act
            var expected = context.Homes.Join(context.HomeUsers,
                home => home.Id,
                homeUser => homeUser.HomeId,
                (home, homeUser) => new { Home = home, HomeUser = homeUser })
                .Where(homeAndUser => homeAndUser.HomeUser.UserId == userIds[0]);
            var request = new GetHomesOfUserRequest() { RequestUserId = userIds[0] };
            var actual = homeService.GetHomesOfUser(request);

            // assert
            Assert.Equal(expected.Count(), actual.Homes.Count());
        }

        [Fact]
        public void Home_GetHomesOfUser_NotFound()
        {
            // arrange
            var context = GetContext();
            IHomeService homeService = new HomeService(context);
            context.HomeUsers.RemoveRange(context.HomeUsers.Where(q => q.UserId == userIds[0]));
            context.SaveChanges();

            // act
            var request = new GetHomesOfUserRequest() { RequestUserId = userIds[0] };
            var actual = homeService.GetHomesOfUser(request);

            // assert
            Assert.Empty(actual.Homes);
        }
    }
}

using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using System;
using System.Linq;
using Xunit;

namespace HomeInv.Business.Tests
{
    public class HomeTests : TestBase, IDisposable
    {
        public void Dispose()
        {
        }

        protected override void SeedData()
        {
            var context = GetContext();
            IHomeService homeService = new HomeService(context);

            HomeEntity homeEntity = new HomeEntity()
            {
                Name = "seed 1",
                Description = "seed desc 1"
            };
            homeService.CreateHome(homeEntity, userIds[0]);

            homeEntity = new HomeEntity()
            {
                Name = "seed 2",
                Description = "seed desc 2"
            };
            homeService.CreateHome(homeEntity, userIds[1]);

            homeEntity = new HomeEntity()
            {
                Name = "seed 3",
                Description = "seed desc 3"
            };
            homeService.CreateHome(homeEntity, userIds[2]);
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
            var expected = homeService.CreateHome(homeEntity, userIds[0]);
            var actual = context.Homes.Find(4);

            // assert
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(userIds[0], actual.InsertUserId);
        }

        [Fact]
        public void Home_GetHomesOfUser_Ok()
        {
            // arrange
            var context = GetContext();
            IHomeService homeService = new HomeService(context);
            HomeEntity homeEntity = new HomeEntity()
            {
                Name = "test home 1",
                Description = "test desc 1"
            };
            homeService.CreateHome(homeEntity, userIds[0]);

            // act
            var expected = context.Homes.Where(q => q.InsertUserId == userIds[0]);
            var actual = homeService.GetHomesOfUser(userIds[0]);

            // assert
            Assert.Equal(expected.Count(), actual.Count());
        }

        [Fact]
        public void Home_GetHomesOfUser_NotFound()
        {
            // arrange
            var context = GetContext();
            IHomeService homeService = new HomeService(context);
            context.Homes.RemoveRange(context.Homes.Where(q => q.InsertUserId == userIds[0]));
            context.SaveChanges();

            // act
            var actual = homeService.GetHomesOfUser(userIds[0]);

            // assert
            Assert.Empty(actual);
        }
    }
}

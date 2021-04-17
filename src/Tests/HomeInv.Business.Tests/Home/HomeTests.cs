using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
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
            HomeEntity homeEntity = new HomeEntity()
            {
                Name = "test home 1",
                Description = "test desc 1"
            };
            var request = new CreateHomeRequest() { HomeEntity = homeEntity, RequestUserId = userIds[0] };
            homeService.CreateHome(request);

            // act
            var expected = context.Homes.Where(q => q.InsertUserId == userIds[0]);
            var request2 = new GetHomesOfUserRequest() { RequestUserId = userIds[0] };
            var actual = homeService.GetHomesOfUser(request2);

            // assert
            Assert.Equal(expected.Count(), actual.Homes.Count());
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
            var request2 = new GetHomesOfUserRequest() { RequestUserId = userIds[0] };
            var actual = homeService.GetHomesOfUser(request2);

            // assert
            Assert.Empty(actual.Homes);
        }
    }
}

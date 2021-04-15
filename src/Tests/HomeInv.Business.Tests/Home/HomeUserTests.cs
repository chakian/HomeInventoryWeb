using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Common.ServiceContracts.HomeUser;
using System;
using System.Linq;
using Xunit;

namespace HomeInv.Business.Tests
{
    public class HomeUserTests : TestBase, IDisposable
    {
        public void Dispose()
        {
        }

        protected override void SeedData()
        {
            var context = GetContext();
            IHomeService homeService = new HomeService(context);
            IHomeUserService homeUserService = new HomeUserService(context);

            HomeEntity homeEntity = new HomeEntity()
            {
                Name = "seed 1",
                Description = "seed desc 1"
            };
            var home = homeService.CreateHome(new CreateHomeRequest() { HomeEntity = homeEntity, RequestUserId = userIds[0] });
            homeUserService.InsertHomeUser(new InsertHomeUserRequest() { HomeId = home.HomeEntity.Id, UserId = userIds[0], Role = "owner", RequestUserId = userIds[0] });

            homeEntity = new HomeEntity()
            {
                Name = "seed 2",
                Description = "seed desc 2"
            };
            home = homeService.CreateHome(new CreateHomeRequest() { HomeEntity = homeEntity, RequestUserId = userIds[1] });
            homeUserService.InsertHomeUser(new InsertHomeUserRequest() { HomeId = home.HomeEntity.Id, UserId = userIds[1], Role = "owner", RequestUserId = userIds[0] });

            homeEntity = new HomeEntity()
            {
                Name = "seed 3",
                Description = "seed desc 3"
            };
            home = homeService.CreateHome(new CreateHomeRequest() { HomeEntity = homeEntity, RequestUserId = userIds[2] });
            homeUserService.InsertHomeUser(new InsertHomeUserRequest() { HomeId = home.HomeEntity.Id, UserId = userIds[2], Role = "owner", RequestUserId = userIds[0] });
        }

        [Fact]
        public void HomeUser_Insert_Ok()
        {
            // arrange
            var context = GetContext();
            IHomeUserService homeUserService = new HomeUserService(context);
            var homeId = context.Homes.First().Id;
            var userId = userIds[1];
            var role = "owner";

            // act
            var actual = homeUserService.InsertHomeUser(new InsertHomeUserRequest() { HomeId = homeId, UserId = userId, Role = role, RequestUserId = userIds[0] });

            // assert
            Assert.True(actual.IsSuccessful);
        }

        [Fact]
        public void Home_Insert_Fail_Exists()
        {
            // arrange
            var context = GetContext();
            IHomeUserService homeUserService = new HomeUserService(context);
            var homeId = context.Homes.First().Id;
            var userId = userIds[0];
            var role = "owner";

            // act
            var actual = homeUserService.InsertHomeUser(new InsertHomeUserRequest() { HomeId = homeId, UserId = userId, Role = role, RequestUserId = userIds[0] });

            // assert
            Assert.False(actual.IsSuccessful);
        }
    }
}

using HomeInv.Common.Interfaces.Services;
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

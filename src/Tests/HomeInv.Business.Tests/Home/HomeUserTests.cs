using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.HomeUser;
using HomeInv.Language;
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
            Assert.Equal(Resources.UserIsAlreadyInThatHome, actual.Result.Messages[0].Text);
        }

        [Fact]
        public void Home_Insert_Fail_UserHasAHome()
        {
            // arrange
            var context = GetContext();
            IHomeUserService homeUserService = new HomeUserService(context);
            var homeId = context.Homes.Add(new Persistence.Dbo.Home()
            {
                Name = "test new home"
            }).Entity.Id;
            context.SaveChanges();
            var userId = userIds[0];
            var role = "owner";

            // act
            var actual = homeUserService.InsertHomeUser(new InsertHomeUserRequest() { HomeId = homeId, UserId = userId, Role = role, RequestUserId = userIds[0] });

            // assert
            Assert.False(actual.IsSuccessful);
            Assert.Equal(Resources.UserAlreadyHasAHome, actual.Result.Messages[0].Text);
        }
    }
}

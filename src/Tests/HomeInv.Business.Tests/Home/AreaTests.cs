//using HomeInv.Common.Entities;
//using HomeInv.Common.Interfaces.Services;
//using HomeInv.Common.ServiceContracts.Area;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace HomeInv.Business.Tests.Home
//{
//    public class AreaTests : TestBase, IDisposable
//    {
//        public void Dispose()
//        {
//        }

//        protected override void SeedData()
//        {
//        }

//        [Fact]
//        public void Area_Create_Ok()
//        {
//            // arrange
//            var context = GetContext();
//            IAreaService areaService = new AreaService(context);
//            var areaEntity = new AreaEntity()
//            {
//                Name = "test home 1",
//                Description = "test desc 1"
//            };

//            // act
//            var expected = context.Homes.Find(4);
//            string actualInsertUserId = expected.InsertUserId;

//            var request = new CreateAreaRequest() { RequestUserId = userIds[0] };
//            var actual = areaService.CreateArea(request);

//            // assert
//            Assert.Equal(expected.Name, actual.AreaEntity.Name);
//            Assert.Equal(userIds[0], actualInsertUserId);
//        }
//    }
//}

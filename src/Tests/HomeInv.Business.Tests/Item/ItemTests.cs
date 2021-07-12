//using HomeInv.Common.Entities;
//using HomeInv.Common.Interfaces.Services;
//using HomeInv.Common.ServiceContracts.Item;
//using HomeInv.Language;
//using System;
//using System.Linq;
//using Xunit;

//namespace HomeInv.Business.Tests
//{
//    public class ItemTests : TestBase, IDisposable
//    {
//        public void Dispose()
//        {
//        }

//        protected override void SeedData()
//        {
//            var context = GetContext();

//            var itemId = context.Items.Add(new Persistence.Dbo.Item()
//            {
//                Name = "selpak"
//            }).Entity.Id;
//            context.ItemStocks.Add(new Persistence.Dbo.ItemStock()
//            {
//                ItemId = itemId,
//                AreaId = context.Areas.First().Id,
//                Quantity = 1
//            });

//            context.SaveChanges();
//        }

//        [Fact]
//        public void Item_Create_Ok()
//        {
//            // arrange
//            var context = GetContext();
//            IItemService itemService = new ItemService(context);
//            ItemEntity itemEntity = new ItemEntity()
//            {
//                Name = "test item 1",
//                Description = "test desc 1"
//            };

//            // act
//            var request = new CreateItemRequest() { ItemEntity = itemEntity, RequestUserId = userIds[0] };
//            var actual = itemService.CreateItem(request);
//            var expected = context.Items.Find(2);

//            // assert
//            Assert.Equal(expected.Name, actual.ItemEntity.Name);
//            Assert.Equal(expected.InsertUserId, userIds[0]);
//        }

//        [Fact]
//        public void Item_Create_Error_EmptyName()
//        {
//            // arrange
//            var context = GetContext();
//            IItemService itemService = new ItemService(context);
//            ItemEntity itemEntity = new ItemEntity()
//            {
//                //Name = "",
//                Description = "test desc 1"
//            };

//            // act
//            var request = new CreateItemRequest() { ItemEntity = itemEntity, RequestUserId = userIds[0] };
//            var actual = itemService.CreateItem(request);

//            // assert
//            Assert.False(actual.IsSuccessful);
//            Assert.Equal("İsim boş olamaz!", actual.Result.Messages[0].Text);
//        }

//        [Fact]
//        public void Item_Create_Error_Exists()
//        {
//            // arrange
//            var context = GetContext();
//            IItemService itemService = new ItemService(context);
//            ItemEntity itemEntity = new ItemEntity()
//            {
//                Name = "selpak",
//                Description = "test desc 1"
//            };

//            // act
//            var request = new CreateItemRequest() { ItemEntity = itemEntity, RequestUserId = userIds[0] };
//            var actual = itemService.CreateItem(request);

//            // assert
//            Assert.False(actual.IsSuccessful);
//            Assert.Equal(Resources.ItemNameExists, actual.Result.Messages[0].Text);
//        }
//    }
//}

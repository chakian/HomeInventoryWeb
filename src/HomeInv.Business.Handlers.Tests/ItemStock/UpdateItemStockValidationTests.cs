using HomeInv.Business.Handlers.Tests.Base;
using HomeInv.Common.Constants;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HomeInv.Business.Handlers.Tests
{
    public class UpdateItemStockValidationTests : TestBase, IDisposable
    {
        public void Dispose()
        {
        }

        [Fact]
        public void UpdateItemStock_Invalid_ItemDefinitionIdEmpty()
        {
            // arrange
            var handlerRequest = new UpdateItemStockRequest()
            {
                ItemStockActionTypeId = (int)ItemStockActionTypeEnum.Purchased,
                AreaId = 1,
                SizeUnitId = 2,
                Size = 3,
            };
            var handler = new UpdateItemStockHandler(_context);
            var expectedError = Language.Resources.ItemStock_Error_ItemDefinitionIdEmpty;

            // act
            var response = handler.Execute(handlerRequest);

            // assert
            Assert.Equal(expectedError, response.Result.ToString());
        }

        [Fact]
        public void UpdateItemStock_Invalid_AreaIdEmpty()
        {
            // arrange
            var handlerRequest = new UpdateItemStockRequest()
            {
                ItemStockActionTypeId = (int)ItemStockActionTypeEnum.Purchased,
                ItemDefinitionId = 1,
                SizeUnitId = 2,
                Size = 3,
            };
            var handler = new UpdateItemStockHandler(_context);

            var expectedError = Language.Resources.ItemStock_Error_AreaIdEmpty;

            // act
            var response = handler.Execute(handlerRequest);

            // assert
            Assert.Equal(expectedError, response.Result.ToString());
        }

        [Fact]
        public void UpdateItemStock_Invalid_SizeUnitIdEmpty()
        {
            // arrange
            var handlerRequest = new UpdateItemStockRequest()
            {
                ItemStockActionTypeId = (int)ItemStockActionTypeEnum.Purchased,
                ItemDefinitionId = 1,
                AreaId = 1,
                Size = 3,
            };
            var handler = new UpdateItemStockHandler(_context);

            var expectedError = Language.Resources.ItemStock_Error_SizeUnitIdEmpty;

            // act
            var response = handler.Execute(handlerRequest);

            // assert
            Assert.Equal(expectedError, response.Result.ToString());
        }

        [Fact]
        public void UpdateItemStock_Invalid_SizeEmpty()
        {
            // arrange
            var handlerRequest = new UpdateItemStockRequest()
            {
                ItemStockActionTypeId = (int)ItemStockActionTypeEnum.Purchased,
                ItemDefinitionId = 1,
                AreaId = 1,
                SizeUnitId = 2
            };
            var handler = new UpdateItemStockHandler(_context);

            var expectedError = Language.Resources.ItemStock_Error_SizeEmpty;

            // act
            var response = handler.Execute(handlerRequest);

            // assert
            Assert.Equal(expectedError, response.Result.ToString());
        }

        [Theory]
        [InlineData(ItemStockActionTypeEnum.Purchased)]
        [InlineData(ItemStockActionTypeEnum.GiftedIn)]
        public void UpdateItemStock_Valid_ForInsert(ItemStockActionTypeEnum actionType)
        {
            // arrange
            var handlerRequest = new UpdateItemStockRequest()
            {
                ItemStockActionTypeId = (int)actionType,
                ItemDefinitionId = 1,
                AreaId = 1,
                SizeUnitId = 2,
                Size = 3,
            };
            var handler = new UpdateItemStockHandler(_context);

            // act
            var response = handler.Execute(handlerRequest);

            // assert
            Assert.True(response.IsSuccessful);
        }

        [Fact]
        public void UpdateItemStock_Valid_ForUpdate()
        {
            // arrange
            var definition = new ItemDefinition()
            {
                Name = ""
            };
            _context.ItemDefinitions.Add(definition);

            var area = new Area()
            {
                Name = ""
            };
            _context.Areas.Add(area);

            var stock = new ItemStock()
            {
                ItemDefinitionId = definition.Id,
                AreaId = area.Id,
            };
            _context.ItemStocks.Add(stock);
            _context.SaveChanges();

            var handlerRequest = new UpdateItemStockRequest()
            {
                ItemStockActionTypeId = (int)ItemStockActionTypeEnum.Consumed,
                ItemStockId = stock.Id,
                ItemDefinitionId = definition.Id,
                AreaId = area.Id,
                Size = 3,
            };
            var handler = new UpdateItemStockHandler(_context);

            // act
            var response = handler.Execute(handlerRequest);

            // assert
            Assert.True(response.IsSuccessful);
        }
    }
}

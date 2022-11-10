using HomeInv.Business.Handlers.Tests.Base;
using HomeInv.Common.Constants;
using HomeInv.Common.ServiceContracts.ItemStock;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HomeInv.Business.Handlers.Tests
{
    public class UpdateItemStock_ActionTypeRelatedTests : TestBase, IDisposable
    {
        public void Dispose()
        {
        }

        [Theory]
        [InlineData(ItemStockActionTypeEnum.Consumed)]
        [InlineData(ItemStockActionTypeEnum.Sold)]
        [InlineData(ItemStockActionTypeEnum.Dismissed)]
        [InlineData(ItemStockActionTypeEnum.Broken)]
        [InlineData(ItemStockActionTypeEnum.Lost)]
        [InlineData(ItemStockActionTypeEnum.GiftedOut)]
        public void UpdateItemStock_Invalid_ItemStockIdEmptyOnUpdateAction(ItemStockActionTypeEnum actionType)
        {
            // arrange
            var handlerRequest = new UpdateItemStockRequest()
            {
                ItemStockActionTypeId = (int)actionType,
                ItemDefinitionId = 1,
                AreaId = 2,
                SizeUnitId = 3,
                Size = 4,
            };
            var handler = new UpdateItemStockHandler(_context);
            var expectedError = Language.Resources.ItemStock_Error_ItemStockIdEmptyOnUpdateAction;

            // act
            var response = handler.Execute(handlerRequest);

            // assert
            Assert.Equal(expectedError, response.Result.ToString());
        }
    }
}

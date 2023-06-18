using HomeInv.Business.Handlers.Tests.Base;
using HomeInv.Common.Constants;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Common.ServiceContracts.ItemStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeInv.Business.Handlers.Tests
{
    public class UpdateItemStockInsertTests : TestBase, IDisposable
    {
        public void Dispose()
        {
        }

        [Fact]
        public void Insert_ItemStock_Ok()
        {
            // arrange
            var handlerRequest = new UpdateItemStockRequest()
            {
                ItemStockActionTypeId = (int)ItemStockActionTypeEnum.Purchased,
                ItemDefinitionId = 1,
                AreaId = 1,
                SizeUnitId = 2,
                Size = 3,
            };
            var handler = new UpdateItemStockHandler(_context);

            var expectedStockId = 1;

            // act
            handler.Execute(handlerRequest);

            var actual = _context.ItemStocks.First();

            // assert
            Assert.Equal(expectedStockId, actual.Id);
        }
    }
}

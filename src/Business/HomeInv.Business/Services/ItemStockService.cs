using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business.Services
{
    public class ItemStockService : AuditableServiceBase<ItemStock, ItemStockEntity>, IItemStockService<ItemStock>
    {
        public ItemStockService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override ItemStockEntity ConvertDboToEntity(ItemStock dbo)
        {
            var entity = new ItemStockEntity()
            {
                Id = dbo.Id,
                ItemDefinitionId = dbo.ItemDefinitionId,
                Quantity = dbo.RemainingAmount,
                AreaId = dbo.AreaId,
                ExpirationDate = (dbo.ExpirationDate.HasValue ? dbo.ExpirationDate.Value : DateTime.MinValue)
            };
            if (dbo.Area != null) { entity.AreaName = dbo.Area.Name; }
            return entity;
        }

        public GetItemStocksByItemDefinitionIdsResponse GetItemStocksByItemDefinitionIds(GetItemStocksByItemDefinitionIdsRequest request)
        {
            var response = new GetItemStocksByItemDefinitionIdsResponse() { ItemStocks = new List<ItemStockEntity>() };

            var stocks = context.ItemStocks
                .Include(stock => stock.Area)
                .Where(stock => request.ItemDefinitionIdList.Contains(stock.ItemDefinitionId));

            foreach (var stock in stocks)
            {
                response.ItemStocks.Add(ConvertDboToEntity(stock));
            }

            return response;
        }

        public GetSingleItemStockResponse GetSingleItemStock(GetSingleItemStockRequest request)
        {
            var response = new GetSingleItemStockResponse() { Stock = new ItemStockEntity() };

            var stock = context.ItemStocks
                .SingleOrDefault(stock => stock.Id == request.ItemStockId);

            if (stock != null)
            {
                response.Stock = ConvertDboToEntity(stock);
            }

            return response;
        }
    }
}

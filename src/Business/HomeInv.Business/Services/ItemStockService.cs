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
                AreaName = dbo.Area.Name,
                ExpirationDate = (dbo.ExpirationDate.HasValue ? dbo.ExpirationDate.Value : DateTime.MinValue),
                SizeUnitId = dbo.SizeUnitId,
                SizeUnitName = dbo.SizeUnit.Name
            };
            return entity;
        }

        public GetItemStocksByItemDefinitionIdsResponse GetItemStocksByItemDefinitionIds(GetItemStocksByItemDefinitionIdsRequest request)
        {
            var response = new GetItemStocksByItemDefinitionIdsResponse() { ItemStocks = new List<ItemStockEntity>() };

            var stocks = context.ItemStocks
                .Include(stock => stock.Area)
                .Include(stock => stock.SizeUnit)
                .Where(stock => request.ItemDefinitionIdList.Contains(stock.Id));

            foreach (var stock in stocks)
            {
                response.ItemStocks.Add(ConvertDboToEntity(stock));
            }

            return response;
        }
    }
}

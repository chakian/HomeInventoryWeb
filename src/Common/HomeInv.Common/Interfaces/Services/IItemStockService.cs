using HomeInv.Common.Models;
using HomeInv.Common.ServiceContracts.ItemStock;
using System.Collections.Generic;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IItemStockService<D> : IItemStockService, IServiceBase<D, Entities.ItemStockEntity>
        where D : class
    {
    }

    public interface IItemStockService : IServiceBase
    {
        GetItemStocksByItemDefinitionIdsResponse GetItemStocksByItemDefinitionIds(GetItemStocksByItemDefinitionIdsRequest request);
        GetSingleItemStockResponse GetSingleItemStock(GetSingleItemStockRequest request);
        Dictionary<int, List<StockStatus>> CheckStocksPrepareShoppingListAndSendEmail(int? homeId);
    }
}

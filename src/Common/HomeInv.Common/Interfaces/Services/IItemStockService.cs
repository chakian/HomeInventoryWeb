using HomeInv.Common.Models;
using HomeInv.Common.ServiceContracts.ItemStock;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IItemStockService<D> : IItemStockService, IServiceBase<D, Entities.ItemStockEntity>
        where D : class
    {
    }

    public interface IItemStockService : IServiceBase
    {
        Task<GetItemStocksByItemDefinitionIdsResponse> GetItemStocksByItemDefinitionIdsAsync(GetItemStocksByItemDefinitionIdsRequest request, CancellationToken ct);
        Task<Dictionary<int, List<StockStatus>>> CheckStocksPrepareShoppingListAndSendEmailAsync(int? homeId, CancellationToken ct);
    }
}

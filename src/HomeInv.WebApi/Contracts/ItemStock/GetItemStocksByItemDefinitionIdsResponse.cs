using HomeInv.Common.Entities;

namespace HomeInv.WebApi.Contracts.ItemStock;

public class GetItemStocksResponse : BaseResponse
{
    public List<ItemStockEntity> ItemStocks { get; set; } = new();
}

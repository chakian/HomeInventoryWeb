using HomeInv.Common.Models;

namespace HomeInv.WebApi.Contracts.ItemStock;

public class GetItemStockStatusByHomeIdResponse : BaseResponse
{
    public List<StockStatus> StockStatus { get; set; } = new();
}

using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.ItemStock
{
    public class GetSingleItemStockResponse : BaseResponse
    {
        public ItemStockEntity Stock { get; set; }
    }
}

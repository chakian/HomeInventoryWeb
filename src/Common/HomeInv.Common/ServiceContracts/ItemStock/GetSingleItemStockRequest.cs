namespace HomeInv.Common.ServiceContracts.ItemStock
{
    public class GetSingleItemStockRequest : BaseRequest
    {
        public int ItemStockId { get; set; }
    }
}

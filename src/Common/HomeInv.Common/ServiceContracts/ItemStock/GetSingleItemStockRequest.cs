namespace HomeInv.Common.ServiceContracts.ItemStock
{
    public class GetSingleItemStockRequest : BaseHomeRelatedRequest
    {
        public int ItemStockId { get; set; }
    }
}

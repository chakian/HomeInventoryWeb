using System;

namespace HomeInv.Common.ServiceContracts.ItemStock
{
    public class UpdateRemainingStockAmountRequest : BaseRequest
    {
        public int ItemStockActionTypeId { get; set; }
        public int ItemStockId { get; set; }
        
        // For updating via Remaining Amount
        public decimal RemainingAmount { get; set; }

        // For updating via Consumed Amount
        public decimal ConsumedAmount { get; set; }

        // For updating via New Purchase or Gifted In
        public decimal EntryAmount { get; set; }
        public decimal Price { get; set; }

        // Generic data
        public DateTime ActionDate { get; set; }
        public string ActionTarget { get; set; }
    }
}

using System;

namespace HomeInv.Common.ServiceContracts.ItemStock
{
    public class UpdateRemainingStockAmountRequest : BaseRequest
    {
        public int ItemStockActionTypeId { get; set; }
        public int ItemStockId { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionTarget { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace HomeInv.Common.ServiceContracts.ItemStock
{
    public class UpdateItemStockRequest : BaseHomeRelatedRequest
    {
        [Required]
        public int ItemStockActionTypeId { get; set; }

        public int ItemStockId { get; set; }
        
        [Required]
        public int ItemDefinitionId { get; set; }

        [Required]
        public int AreaId { get; set; }

        [Required]
        public int SizeUnitId { get; set; }

        [Required]
        public decimal Size { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public decimal ConsumedAmount { get; set; }

        public DateTime ActionDate { get; set; }

        public string ActionTarget { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; } = "TL";
    }
}

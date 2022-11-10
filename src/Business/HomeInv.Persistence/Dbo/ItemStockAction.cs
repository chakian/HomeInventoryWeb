using System;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class ItemStockAction : BaseAuditableDbo
    {
        [Required]
        public int ItemStockId { get; set; }
        public virtual ItemStock ItemStock { get; set; }

        public decimal Size { get; set; }

        [Required]
        public int ItemStockActionTypeId { get; set; }
        public virtual ItemStockActionType ItemStockActionType { get; set; }

        [Required]
        public DateTime ActionDate { get; set; }

        public string ActionTarget { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }
    }
}

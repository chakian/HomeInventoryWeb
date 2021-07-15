using System;

namespace HomeInv.Common.Entities
{
    public class ItemStockEntity : EntityBase
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }

        public decimal Quantity { get; set; }
    }

    public class ConsumableItemStockEntity : ItemStockEntity
    {
        public DateTime ExpirationDate { get; set; }
    }
}

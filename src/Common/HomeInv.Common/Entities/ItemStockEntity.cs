using System;

namespace HomeInv.Common.Entities
{
    public class ItemStockEntity : EntityBase
    {
        public int ItemDefinitionId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }

        public int AreaId { get; set; }
        public string AreaName { get; set; }

        public decimal Quantity { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int SizeUnitId { get; set; }
        public string SizeUnitName { get; set; }
    }
}

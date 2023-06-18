using System;

namespace HomeInv.Common.ServiceContracts.ItemStock;

public class SmartUpdateItemStockRequest : BaseHomeRelatedRequest
{
    public ItemDefinition ItemDefinitionDetail { get; set; } = new ItemDefinition();

    public ItemStock ItemStockDetail { get; set; } = new ItemStock();

    public DateTime ActionDate { get; set; } = DateTime.UtcNow;
    public string ActionTarget { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "TL";

    public class ItemDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int SizeUnitId { get; set; }
        public bool IsExpirable { get; set; }
        public string ImageName { get; set; }
    }

    public class ItemStock
    {
        public int Id { get; set; }
        public decimal RemainingAmount { get; set; }
        public int AreaId { get; set; }
        public DateTime? ExpirationDate { get; set; } = DateTime.MinValue;
    }
}

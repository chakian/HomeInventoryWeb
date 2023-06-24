using HomeInv.Common.Entities;

namespace HomeInv.Common.Models;

public class StockStatus
{
    public int ItemDefinitionId { get; set; }
    public string ItemDefinitionName { get; set; }
    public int ItemStockId { get; set; }
    public decimal? CurrentStock { get; set; }
    public SizeUnitEntity SizeUnit { get; set; }
    public string Note { get; set; }
    public decimal NeededAmount { get; set; }
    public StockNeed CurrentNeed { get; set; }
}

public enum StockNeed
{
    Needed,
    NotSure,
    Fine
}

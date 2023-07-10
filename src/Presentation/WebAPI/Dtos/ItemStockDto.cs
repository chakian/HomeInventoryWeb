namespace WebAPI.Dtos;

public class ItemStockDto
{
    public int StockId { get; set; }
    public int ItemDefinitionId { get; set; }
    public string ImageName { get; set; } = default!;

    public int SizeUnitId { get; set; }
    public string SizeUnitName { get; set; } = default!;

    public string ItemName { get; set; } = default!;
    public string ItemDescription { get; set; } = default!;

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public string CategoryParentNames { get; set; } = default!;

    public DateTime? ExpirationDate { get; set; }
    public decimal CurrentStockAmount { get; set; }

    public ItemStockDto Clone()
    {
        return new ItemStockDto()
        {
            StockId = StockId,
            ItemDefinitionId = ItemDefinitionId,
            ImageName = ImageName,
            SizeUnitId = SizeUnitId,
            SizeUnitName = SizeUnitName,
            ItemName = ItemName,
            ItemDescription = ItemDescription,
            CategoryId = CategoryId,
            CategoryName = CategoryName,
            CategoryParentNames = CategoryParentNames,
            ExpirationDate = ExpirationDate,
            CurrentStockAmount = CurrentStockAmount,
        };
    }
}

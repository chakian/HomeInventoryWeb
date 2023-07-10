namespace WebAPI.Dtos;

public class ItemStockListDto
{
    public ItemStockListDto()
    {
        ItemStocks = new List<ItemStockDto>();
    }

    public List<ItemStockDto> ItemStocks { get; set; } = default!;
}

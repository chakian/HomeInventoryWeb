namespace HomeInv.WebApi.Contracts.ItemStock;

public class GetItemStocksByItemDefinitionIdsRequest : BaseHomeRelatedRequest
{
    public List<int> ItemDefinitionIds { get; set; } = new();
}

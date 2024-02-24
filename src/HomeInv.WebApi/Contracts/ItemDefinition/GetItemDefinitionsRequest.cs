namespace HomeInv.WebApi.Contracts.ItemDefinition;

public sealed class GetItemDefinitionsRequest : BaseHomeRelatedRequest
{
    public int AreaId { get; set; }

    public int CategoryId { get; set; }
    
    public bool IncludeInactive { get; set; }
}

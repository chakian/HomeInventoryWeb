namespace HomeInv.WebApi.Contracts.ItemDefinition;

public class CreateItemDefinitionRequest : BaseHomeRelatedRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int SizeUnitId { get; set; }

    public int CategoryId { get; set; }

    public bool IsExpirable { get; set; }

    public string? ImageBase64 { get; set; }
}

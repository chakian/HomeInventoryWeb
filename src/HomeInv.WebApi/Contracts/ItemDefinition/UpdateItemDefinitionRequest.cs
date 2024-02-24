namespace HomeInv.WebApi.Contracts.ItemDefinition;

public class UpdateItemDefinitionRequest : BaseHomeRelatedRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int SizeUnitId { get; set; }

    public int CategoryId { get; set; }

    public bool IsExpirable { get; set; }

    public string? ImageBase64 { get; set; }
}

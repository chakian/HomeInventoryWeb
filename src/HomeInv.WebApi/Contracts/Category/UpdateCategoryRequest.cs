namespace HomeInv.WebApi.Contracts.Category;

public class UpdateCategoryRequest : BaseHomeRelatedRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public int? ParentCategoryId { get; set; }
}

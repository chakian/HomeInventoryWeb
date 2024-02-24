namespace HomeInv.WebApi.Contracts.Category;

public class CreateCategoryRequest : BaseHomeRelatedRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int? ParentCategoryId { get; set; }
}

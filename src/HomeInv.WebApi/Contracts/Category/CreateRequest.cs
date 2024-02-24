namespace HomeInv.WebApi.Contracts.Category;

public class CreateRequest : BaseHomeRelatedRequest
{
    public string Name { get; set; }

    public string Description { get; set; }
}
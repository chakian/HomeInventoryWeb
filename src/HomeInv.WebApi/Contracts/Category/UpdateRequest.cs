namespace HomeInv.WebApi.Contracts.Category;

public class UpdateRequest : BaseHomeRelatedRequest
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
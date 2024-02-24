namespace HomeInv.WebApi.Contracts.Home;

public class CreateHomeRequest : BaseRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}

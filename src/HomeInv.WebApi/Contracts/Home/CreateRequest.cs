namespace HomeInv.WebApi.Contracts.Home;

public class CreateRequest : BaseRequest
{
    public string Name { get; set; }

    public string Description { get; set; }
}
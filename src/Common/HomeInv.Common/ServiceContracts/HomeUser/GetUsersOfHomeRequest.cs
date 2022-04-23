namespace HomeInv.Common.ServiceContracts.HomeUser
{
    public class GetUsersOfHomeRequest : BaseRequest
    {
        public int HomeId { get; set; }
    }
}

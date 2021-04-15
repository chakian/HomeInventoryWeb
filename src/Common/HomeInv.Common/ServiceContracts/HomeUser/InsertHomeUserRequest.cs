namespace HomeInv.Common.ServiceContracts.HomeUser
{
    public class InsertHomeUserRequest : BaseRequest
    {
        public int HomeId { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}

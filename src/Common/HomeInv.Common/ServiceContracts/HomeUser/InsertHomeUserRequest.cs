namespace HomeInv.Common.ServiceContracts.HomeUser
{
    public class InsertHomeUserRequest : BaseHomeRelatedRequest
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}

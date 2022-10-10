namespace HomeInv.Common.ServiceContracts.AreaUser
{
    public class InsertAreaUserRequest : BaseHomeRelatedRequest
    {
        public int AreaId { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}

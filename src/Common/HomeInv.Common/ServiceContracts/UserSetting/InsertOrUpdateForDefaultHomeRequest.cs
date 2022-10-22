namespace HomeInv.Common.ServiceContracts.UserSetting
{
    public class InsertOrUpdateForDefaultHomeRequest : BaseRequest
    {
        public string UserId { get; set; }
        public int DefaultHomeId { get; set; }
    }
}

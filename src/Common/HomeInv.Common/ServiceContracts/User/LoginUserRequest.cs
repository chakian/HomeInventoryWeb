using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts
{
    public class LoginUserRequest : BaseRequest
    {
        public HIUser User { get; set; }
    }
}

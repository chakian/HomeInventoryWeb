using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts
{
    public class RegisterUserRequest : BaseRequest
    {
        public HIUser User { get; set; }
    }
}

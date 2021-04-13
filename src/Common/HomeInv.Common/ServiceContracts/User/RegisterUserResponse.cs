using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts
{
    public class RegisterUserResponse : BaseResponse
    {
        public HIUser User { get; set; }
    }
}

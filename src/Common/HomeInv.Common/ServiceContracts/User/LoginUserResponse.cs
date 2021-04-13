using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts
{
    public class LoginUserResponse : BaseResponse
    {
        public HIUser User { get; set; }
    }
}

using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Home
{
    public class GetSingleHomeOfUserResponse : BaseResponse
    {
        public HomeEntity Home { get; set; }
    }
}

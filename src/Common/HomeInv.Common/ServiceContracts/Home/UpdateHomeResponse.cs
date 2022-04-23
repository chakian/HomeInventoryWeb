using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Home
{
    public class UpdateHomeResponse : BaseResponse
    {
        public HomeEntity HomeEntity { get; set; }
    }
}

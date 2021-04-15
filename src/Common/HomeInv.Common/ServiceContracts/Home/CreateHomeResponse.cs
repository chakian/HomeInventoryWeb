using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Home
{
    public class CreateHomeResponse : BaseResponse
    {
        public HomeEntity HomeEntity { get; set; }
    }
}

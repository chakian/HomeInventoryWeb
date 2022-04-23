using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Home
{
    public class UpdateHomeRequest : BaseRequest
    {
        public HomeEntity HomeEntity { get; set; }
    }
}

using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Home
{
    public class CreateHomeRequest : BaseRequest
    {
        public HomeEntity HomeEntity { get; set; }
    }
}

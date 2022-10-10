using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Area
{
    public class CreateAreaRequest : BaseRequest
    {
        public AreaEntity AreaEntity { get; set; }
    }
}

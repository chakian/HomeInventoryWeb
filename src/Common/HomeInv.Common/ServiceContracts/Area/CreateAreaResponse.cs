using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Area
{
    public class CreateAreaResponse : BaseResponse
    {
        public AreaEntity AreaEntity { get; set; }
    }
}

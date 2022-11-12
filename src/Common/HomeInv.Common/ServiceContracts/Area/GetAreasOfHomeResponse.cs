using HomeInv.Common.Entities;
using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.Area
{
    public class GetAreasOfHomeResponse : BaseResponse
    {
        public List<AreaEntity> Areas { get; set; }
    }
}

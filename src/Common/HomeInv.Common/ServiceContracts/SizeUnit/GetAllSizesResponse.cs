using HomeInv.Common.Entities;
using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.SizeUnit
{
    public class GetAllSizesResponse : BaseResponse
    {
        public List<SizeUnitEntity> SizeUnits { get; set; }
    }
}

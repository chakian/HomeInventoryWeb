using HomeInv.Common.Entities;
using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.Home
{
    public class GetHomesOfUserResponse : BaseResponse
    {
        public List<HomeEntity> Homes { get; set; }
    }
}

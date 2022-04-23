using HomeInv.Common.Entities;
using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.HomeUser
{
    public class GetUsersOfHomeResponse : BaseResponse
    {
        public List<HomeUserEntity> HomeUsers { get; set; }
    }
}

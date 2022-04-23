using HomeInv.Common.Entities;
using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.User
{
    public class SearchUserResponse : BaseResponse
    {
        public List<UserEntity> SearchResults { get; set; }
    }
}

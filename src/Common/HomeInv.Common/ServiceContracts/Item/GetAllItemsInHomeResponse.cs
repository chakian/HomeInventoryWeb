using HomeInv.Common.Entities;
using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.Item
{
    public class GetAllItemsInHomeResponse : BaseResponse
    {
        public List<ItemEntity> Items { get; set; }
    }
}

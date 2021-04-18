using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Item
{
    public class CreateItemResponse : BaseResponse
    {
        public ItemEntity ItemEntity { get; set; }
    }
}

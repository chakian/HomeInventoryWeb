using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Item
{
    public class CreateItemRequest : BaseRequest
    {
        public ItemEntity ItemEntity { get; set; }
    }
}

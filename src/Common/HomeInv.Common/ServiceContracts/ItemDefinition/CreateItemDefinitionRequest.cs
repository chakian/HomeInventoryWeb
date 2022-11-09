using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class CreateItemDefinitionRequest : BaseRequest
    {
        public ItemDefinitionEntity ItemEntity { get; set; }
    }
}

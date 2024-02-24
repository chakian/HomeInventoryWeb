using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class CreateItemDefinitionRequest : BaseHomeRelatedRequest
    {
        public ItemDefinitionEntity ItemEntity { get; set; }

        public string ImageBase64 { get; set; }
    }
}

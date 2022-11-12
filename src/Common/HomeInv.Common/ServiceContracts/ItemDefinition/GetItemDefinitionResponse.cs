using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class GetItemDefinitionResponse : BaseResponse
    {
        public ItemDefinitionEntity ItemDefinition { get; set; }
    }
}

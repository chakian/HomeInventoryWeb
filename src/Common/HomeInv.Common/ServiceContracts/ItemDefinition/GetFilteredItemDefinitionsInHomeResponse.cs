using HomeInv.Common.Entities;
using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class GetFilteredItemDefinitionsInHomeResponse : BaseResponse
    {
        public List<ItemDefinitionEntity> Items { get; set; }
    }
}

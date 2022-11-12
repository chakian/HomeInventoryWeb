using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.ItemStock
{
    public class GetItemStocksByItemDefinitionIdsRequest : BaseHomeRelatedRequest
    {
        public List<int> ItemDefinitionIdList { get; set; }
    }
}

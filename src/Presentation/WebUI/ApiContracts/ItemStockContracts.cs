using System;
using System.Collections.Generic;

namespace WebUI.ApiContracts
{
    public class ItemStockGetRequestContract
    {
    }

    public class ItemStockGetResponseContract : ApiResponseBase
    {
        public List<ItemStockDefinition> ItemStocks { get; set; }

        public class ItemStockDefinition
        {
            public int Id { get; set; }

            public int ItemDefinitionId { get; set; }
            public string ItemName { get; set; }
            public string ItemDescription { get; set; }

            public int AreaId { get; set; }
            public string AreaName { get; set; }

            public decimal RemainingAmount { get; set; }

            public DateTime ExpirationDate { get; set; }

            public int SizeUnitId { get; set; }
            public string SizeUnitName { get; set; }
        }
    }
}

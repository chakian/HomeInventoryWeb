﻿using HomeInv.Common.Entities;
using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.ItemStock
{
    public class GetItemStocksByItemDefinitionIdsResponse : BaseResponse
    {
        public List<ItemStockEntity> ItemStocks { get; set; }
    }
}

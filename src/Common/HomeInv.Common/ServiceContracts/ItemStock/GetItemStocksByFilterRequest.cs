using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeInv.Common.ServiceContracts.ItemStock
{
    public class GetItemStocksByFilterRequest : BaseHomeRelatedRequest
    {
        public int CategoryId { get; set; }
    }
}

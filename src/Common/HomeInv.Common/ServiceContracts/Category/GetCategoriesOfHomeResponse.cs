using HomeInv.Common.Entities;
using System.Collections.Generic;

namespace HomeInv.Common.ServiceContracts.Category
{
    public class GetCategoriesOfHomeResponse : BaseResponse
    {
        public List<CategoryEntity> Categories { get; set; }
    }
}

using HomeInv.Common.Entities;

namespace HomeInv.Common.ServiceContracts.Category
{
    public class CreateCategoryRequest : BaseHomeRelatedRequest
    {
        public CategoryEntity CategoryEntity { get; set; }
    }
}

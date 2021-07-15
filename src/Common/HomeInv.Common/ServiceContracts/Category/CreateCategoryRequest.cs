namespace HomeInv.Common.ServiceContracts.Category
{
    public class CreateCategoryRequest : BaseHomeRelatedRequest
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}

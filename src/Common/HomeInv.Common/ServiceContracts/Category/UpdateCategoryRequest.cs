namespace HomeInv.Common.ServiceContracts.Category
{
    public class UpdateCategoryRequest : BaseHomeRelatedRequest
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}

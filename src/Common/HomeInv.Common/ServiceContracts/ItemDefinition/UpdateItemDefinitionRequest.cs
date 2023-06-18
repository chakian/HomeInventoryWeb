namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class UpdateItemDefinitionRequest : BaseHomeRelatedRequest
    {
        public int ItemDefinitionId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string NewImageFileExtension { get; set; }
        public bool IsExpirable { get; set; }
    }
}

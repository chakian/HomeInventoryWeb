namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class UpdateItemDefinitionRequest : BaseHomeRelatedRequest
    {
        public int ItemDefinitionId { get; init; }

        public string Name { get; init; }
        public string Description { get; init; }
        public int CategoryId { get; init; }
        public bool IsExpirable { get; init; }
        public string ImageBase64 { get; init; }
    }
}

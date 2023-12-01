namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class DeleteItemDefinitionRequest : BaseHomeRelatedRequest
    {
        public int ItemDefinitionId { get; set; }
    }
}

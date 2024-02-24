namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class GetAllItemDefinitionsInHomeRequest : BaseHomeRelatedRequest
    {
        public bool IncludeInactive { get; set; } = false;
    }
}

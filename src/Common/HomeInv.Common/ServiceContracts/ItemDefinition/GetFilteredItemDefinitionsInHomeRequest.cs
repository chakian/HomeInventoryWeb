namespace HomeInv.Common.ServiceContracts.ItemDefinition
{
    public class GetFilteredItemDefinitionsInHomeRequest : BaseHomeRelatedRequest
    {
        public int AreaId { get; set; }
        public int CategoryId { get; set; }
        //public bool IncludeItemsOnlyInInventory { get; set; }
        //public bool ExcludeItemsInInventory { get; set; }
    }
}

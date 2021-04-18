namespace HomeInv.Persistence.Dbo
{
    public class ItemStock : BaseAuditableDbo
    {
        public int ItemId { get; set; }
        public int HomeId { get; set; }
        public decimal Quantity { get; set; }
    }
}

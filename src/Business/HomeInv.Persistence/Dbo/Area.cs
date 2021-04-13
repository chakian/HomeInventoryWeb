namespace HomeInv.Persistence.Dbo
{
    public class Area : BaseAuditableDbo
    {
        public int HomeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Home Home { get; set; }
    }
}

namespace HomeInv.Persistence.Dbo
{
    public class Home : BaseAuditableDbo
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

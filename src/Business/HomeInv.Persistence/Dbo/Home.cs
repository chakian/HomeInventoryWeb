using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class Home : BaseAuditableDbo
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

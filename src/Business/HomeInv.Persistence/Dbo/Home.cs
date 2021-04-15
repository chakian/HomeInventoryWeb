using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class Home : BaseAuditableDbo
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

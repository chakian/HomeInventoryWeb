using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class AreaUser : BaseAuditableDbo
    {
        [Required]
        public int AreaId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Role { get; set; }

        public virtual Area Area { get; set; }
        public virtual User User { get; set; }
    }
}

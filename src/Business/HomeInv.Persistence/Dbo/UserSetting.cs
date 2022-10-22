using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeInv.Persistence.Dbo
{
    public class UserSetting : BaseAuditableDbo
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public int DefaultHomeId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public virtual Home DefaultHome { get; set; }
    }
}

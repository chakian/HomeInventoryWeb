using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class HomeUser : BaseAuditableDbo
    {
        [Required]
        public int HomeId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Role { get; set; }
        public string RequestedByUserId { get; set; }

        public virtual Home Home { get; set; }
        public virtual User User { get; set; }
        public virtual User RequestedByUser { get; set; }
    }
}

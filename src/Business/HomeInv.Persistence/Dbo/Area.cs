using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class Area : BaseAuditableDbo
    {
        [Required]
        public int HomeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Home Home { get; set; }

        public virtual IEnumerable<ItemStock> ItemStocks { get; set; }
        public virtual IEnumerable<AreaUser> AreaUsers { get; set; }
    }
}

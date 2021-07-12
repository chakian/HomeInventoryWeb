using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class Home : BaseAuditableDbo
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Description { get; set; }

        //public virtual IEnumerable<Area> Areas { get; set; }

        public virtual IEnumerable<HomeUser> HomeUsers { get; set; }
    }
}

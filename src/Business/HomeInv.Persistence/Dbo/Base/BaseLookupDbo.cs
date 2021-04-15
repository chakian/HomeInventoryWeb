using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class BaseLookupDbo : BaseDbo
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}

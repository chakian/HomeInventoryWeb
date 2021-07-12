using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class SizeUnit : BaseDbo
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsBaseUnit { get; set; }
        public int? BaseUnitId { get; set; }
        public decimal ConversionMultiplierToBase { get; set; }
    }
}

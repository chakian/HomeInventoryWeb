using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class SizeUnitEntity : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsBaseUnit { get; set; }
        public decimal ConversionMultiplierToBase { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class ItemEntity : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int SizeUnitId { get; set; }
        public int SizeUnitText { get; set; }

        public bool IsExpirable { get; set; }
        public bool IsContainer { get; set; }
    }
}

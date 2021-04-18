using System.ComponentModel.DataAnnotations;

namespace HomeInv.Common.Entities
{
    public class ItemEntity : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class ItemStockActionType : BaseDbo
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}

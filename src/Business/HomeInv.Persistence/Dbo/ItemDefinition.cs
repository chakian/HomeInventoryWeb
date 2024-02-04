using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class ItemDefinition : BaseAuditableDbo
    {
        #region Generic properties
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        #endregion

        #region Category
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        #endregion

        #region Size Properties
        public int SizeUnitId { get; set; }
        public virtual SizeUnit SizeUnit { get; set; }
        #endregion

        #region Consumable
        public bool IsExpirable { get; set; }
        #endregion

        public virtual IEnumerable<ItemStock> ItemStocks { get; set; }
        //TODO: Add "Brand" and "Model"
    }
}

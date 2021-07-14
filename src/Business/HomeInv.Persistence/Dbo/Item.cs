using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class Item : BaseAuditableDbo
    {
        #region Generic properties
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        //TODO: ItemPhoto
        #endregion

        #region Category
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        #endregion

        #region Size Properties
        public int SizeUnitId { get; set; }
        public virtual SizeUnit SizeUnit { get; set; }
        public decimal Size { get; set; }
        #endregion

        #region Consumable
        public bool IsExpirable { get; set; }
        #endregion

        #region Container
        public bool IsContainer { get; set; }
        #endregion
    }
}

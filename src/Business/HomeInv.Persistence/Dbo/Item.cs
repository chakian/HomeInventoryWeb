using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class Item : BaseAuditableDbo
    {
        [Required]
        public int HomeId { get; set; }
        #region Generic properties

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        //public int ItemTypeId { get; set; }
        #endregion

        #region Consumable
        //public bool IsConsumable { get; set; }
        //public DateTime ExpirationDate { get; set; }
        #endregion

        #region Container
        //public bool IsContainer { get; set; }
        //public int ContainerId { get; set; }
        #endregion

        public virtual Home Home { get; set; }
    }
}

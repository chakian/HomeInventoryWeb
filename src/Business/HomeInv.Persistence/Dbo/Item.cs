using System;

namespace HomeInv.Persistence.Dbo
{
    public class Item : BaseAuditableDbo
    {
        #region Generic properties
        public string Name { get; set; }

        public string Description { get; set; }
        public int ItemTypeId { get; set; }
        #endregion

        #region Consumable
        public bool IsConsumable { get; set; }
        public DateTime ExpirationDate { get; set; }
        #endregion

        #region Container
        public bool IsContainer { get; set; }
        public int ContainerId { get; set; }
        #endregion
    }
}

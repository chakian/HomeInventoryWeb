using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class ItemStock : BaseAuditableDbo
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        [Required]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }

        #region Container
        public int? ContainerId { get; set; }
        public virtual ItemStock Container { get; set; }
        public virtual IEnumerable<ItemStock> ContainingItems { get; set; }
        #endregion

        #region Consumable
        public DateTime? ExpirationDate { get; set; }
        #endregion
    }
}

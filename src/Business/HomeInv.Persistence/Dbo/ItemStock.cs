using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class ItemStock : BaseAuditableDbo
    {
        [Required]
        public int ItemDefinitionId { get; set; }
        public virtual ItemDefinition ItemDefinition { get; set; }

        [Required]
        public int AreaId { get; set; }
        public virtual Area Area { get; set; }

        #region Size Properties
        public int SizeUnitId { get; set; }
        public virtual SizeUnit SizeUnit { get; set; }
        #endregion
        
        #region Consumable
        public DateTime? ExpirationDate { get; set; }
        #endregion

        public decimal RemainingAmount { get; set; }

        public virtual IEnumerable<ItemStockAction> ItemStockActions { get; set; }
    }
}

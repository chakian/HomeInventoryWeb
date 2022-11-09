using System;
using System.ComponentModel.DataAnnotations;

namespace HomeInv.Persistence.Dbo
{
    public class ItemUnitPrice : BaseDbo
    {
        public int? ItemStockActionId { get; set; }
        public virtual ItemStockAction ItemStockAction { get; set; }

        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Irrelevant if ItemStockActionId is entered.
        /// Required if ItemStockActionId is null.
        /// </summary>
        public int? ItemDefinitionId { get; set; }
        public virtual ItemDefinition ItemDefinition { get; set; }

        /// <summary>
        /// Irrelevant if ItemStockActionId is entered.
        /// </summary>
        public DateTime? PriceDate { get; set; }

        /// <summary>
        /// Irrelevant if ItemStockActionId is entered.
        /// </summary>
        public string Currency { get; set; }
    }
}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace HomeInv.Persistence.Dbo
//{
//    public class ItemStock : BaseAuditableDbo
//    {
//        [Required]
//        public int ItemId { get; set; }

//        public virtual Item Item { get; set; }

//        public int Quantity { get; set; }

//        [Required]
//        public DateTime PurchaseDate { get; set; }

//        #region Size Properties
//        public int SizeUnitId { get; set; }
//        public virtual SizeUnit SizeUnit { get; set; }
//        public decimal Size { get; set; }
//        #endregion

//        [Required]
//        public int AreaId { get; set; }
//        public virtual Area Area { get; set; }

//        #region Container
//        public int? ContainerId { get; set; }
//        public virtual ItemStock Container { get; set; }
//        public virtual IEnumerable<ItemStock> ContainingItems { get; set; }
//        #endregion

//        #region Consumable
//        public DateTime? ExpirationDate { get; set; }
//        #endregion
//    }
//}

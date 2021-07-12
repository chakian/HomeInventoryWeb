using System;

namespace HomeInv.Persistence.Dbo
{
    public class BaseAuditableDbo : BaseDbo
    {
        public string InsertUserId { get; set; }
        public DateTime InsertTime { get; set; }
        
        public string UpdateUserId { get; set; }
        public DateTime? UpdateTime { get; set; }

        #region Related Objects
        public virtual User InsertUser { get; set; }
        public virtual User UpdateUser { get; set; }
        #endregion
    }
}

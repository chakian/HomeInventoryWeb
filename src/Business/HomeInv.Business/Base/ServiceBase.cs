using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;

namespace HomeInv.Business.Base
{
    public abstract class ServiceBase : IServiceBase
    {
        protected readonly HomeInventoryDbContext context;
        public ServiceBase(HomeInventoryDbContext _context)
        {
            context = _context;
        }

        protected T CreateNewAuditableObject<T>(string userId, bool isActive = true)
            where T : BaseAuditableDbo, new()
        {
            T dbo = new T();
            dbo.InsertUserId = userId;
            dbo.InsertTime = DateTime.UtcNow;
            dbo.IsActive = isActive;
            return dbo;
        }

        protected void UpdateAuditableObject<T>(T dbo, string userId)
            where T : BaseAuditableDbo
        {
            dbo.UpdateUserId = userId;
            dbo.UpdateTime = DateTime.UtcNow;
        }
    }
}

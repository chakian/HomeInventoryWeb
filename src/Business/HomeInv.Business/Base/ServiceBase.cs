using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts;
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

        protected T CreateNewAuditableObject<T>(BaseRequest request, bool isActive = true)
            where T : BaseAuditableDbo, new()
        {
            T dbo = new T();
            dbo.InsertUserId = request.RequestUserId;
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

        protected U ConvertBaseDboToEntityBase<T, U>(T dbo)
            where T : BaseDbo
            where U : EntityBase, new()
        {
            U entity = new U();
            entity.Id = dbo.Id;
            return entity;
        }
    }
}

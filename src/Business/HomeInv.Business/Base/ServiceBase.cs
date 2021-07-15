using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Linq;

namespace HomeInv.Business.Base
{
    public abstract class AuditableServiceBase<D, E> : ServiceBase<D, E>, IAuditableServiceBase<D, E>
        where D : BaseAuditableDbo, new()
        where E : EntityBase, new()
    {
        public AuditableServiceBase(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public D CreateNewAuditableObject(BaseRequest request, bool isActive = true)
        {
            D dbo = CreateNewObject(isActive);
            dbo.InsertUserId = request.RequestUserId;
            dbo.InsertTime = DateTime.UtcNow;
            return dbo;
        }

        public void UpdateAuditableObject(D dbo, string userId)
        {
            dbo.UpdateUserId = userId;
            dbo.UpdateTime = DateTime.UtcNow;
        }

        public E ConvertBaseDboToEntityBase(D dbo)
        {
            E entity = new E();
            entity.Id = dbo.Id;
            return entity;
        }
    }
    public abstract class ServiceBase<D, E> : ServiceBase, IServiceBase<D, E>
        where D : BaseDbo, new()
        where E : EntityBase, new()
    {
        public ServiceBase(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public IQueryable<D> GetAllActiveAsQueryable()
        {
            return context.Set<D>().Where(set => set.IsActive);
        }

        public D CreateNewObject(bool isActive = true)
        {
            D dbo = new D();
            dbo.IsActive = isActive;
            return dbo;
        }

        public abstract E ConvertDboToEntity(D dbo);
    }

    public abstract class ServiceBase : IServiceBase
    {
        protected readonly HomeInventoryDbContext context;
        public ServiceBase(HomeInventoryDbContext _context)
        {
            context = _context;
        }
    }
}

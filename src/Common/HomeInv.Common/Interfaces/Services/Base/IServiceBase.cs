using HomeInv.Common.ServiceContracts;
using System.Linq;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IAuditableServiceBase<D, E> : IServiceBase<D, E>
        where D : class, new()
        where E : Entities.EntityBase
    {
        D CreateNewAuditableObject(BaseRequest request, bool isActive = true);
        void UpdateAuditableObject(D dbo, string userId);
        E ConvertBaseDboToEntityBase(D dbo);
    }

    public interface IServiceBase<D, E> : IServiceBase
        where D : class
        where E : Entities.EntityBase
    {
        IQueryable<D> GetAllActiveAsQueryable();
        E ConvertDboToEntity(D dbo);
    }

    public interface IServiceBase { }
}

using HomeInv.Common.ServiceContracts.SizeUnit;

namespace HomeInv.Common.Interfaces.Services
{
    public interface ISizeUnitService<D> : ISizeUnitService, IServiceBase<D, Entities.SizeUnitEntity>
        where D : class
    {
    }

    public interface ISizeUnitService : IServiceBase
    {
        GetAllSizesResponse GetAllSizes(GetAllSizesRequest request);
    }
}

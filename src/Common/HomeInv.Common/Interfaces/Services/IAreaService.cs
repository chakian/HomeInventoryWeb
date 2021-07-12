using HomeInv.Common.ServiceContracts.Area;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IAreaService<D> : IAreaService, IServiceBase<D, Entities.AreaEntity>
        where D : class
    {
    }

    public interface IAreaService : IServiceBase
    {
        CreateAreaResponse CreateArea(CreateAreaRequest request);
        GetAreasOfHomeResponse GetAreasOfHome(GetAreasOfHomeRequest request);
    }
}

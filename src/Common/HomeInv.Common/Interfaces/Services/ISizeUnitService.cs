using System.Threading;
using System.Threading.Tasks;
using HomeInv.Common.ServiceContracts.SizeUnit;

namespace HomeInv.Common.Interfaces.Services
{
    public interface ISizeUnitService<D> : ISizeUnitService, IServiceBase<D, Entities.SizeUnitEntity>
        where D : class
    {
    }

    public interface ISizeUnitService : IServiceBase
    {
        Task<GetAllSizesResponse> GetAllSizesAsync(GetAllSizesRequest request, CancellationToken ct);
    }
}

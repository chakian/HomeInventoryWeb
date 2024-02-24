using System.Threading;
using HomeInv.Common.ServiceContracts.Home;
using System.Threading.Tasks;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IHomeService<D> : IHomeService, IServiceBase<D, Entities.HomeEntity>
        where D : class
    {
    }

    public interface IHomeService : IServiceBase
    {
        Task<CreateHomeResponse> CreateHomeAsync(CreateHomeRequest request, CancellationToken ct);

        Task<GetHomesOfUserResponse> GetHomesOfUserAsync(GetHomesOfUserRequest request, CancellationToken ct);
        
        Task<UpdateHomeResponse> UpdateHomeAsync(UpdateHomeRequest request, CancellationToken ct);
    }
}

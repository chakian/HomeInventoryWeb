using System.Threading;
using System.Threading.Tasks;
using HomeInv.Common.ServiceContracts.HomeUser;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IHomeUserService : IServiceBase
    {
        Task<InsertHomeUserResponse> InsertHomeUserAsync(InsertHomeUserRequest request, CancellationToken ct);
        Task<GetUsersOfHomeResponse> GetUsersOfHomeAsync(GetUsersOfHomeRequest request, CancellationToken ct);
    }

    public interface IHomeUserService<D> : IHomeUserService, IServiceBase<D, Entities.HomeUserEntity>
        where D : class
    {
    }
}

using HomeInv.Common.ServiceContracts.HomeUser;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IHomeUserService : IServiceBase
    {
        InsertHomeUserResponse InsertHomeUser(InsertHomeUserRequest request);
        GetUsersOfHomeResponse GetUsersOfHome(GetUsersOfHomeRequest request);
    }

    public interface IHomeUserService<D> : IHomeUserService, IServiceBase<D, Entities.HomeUserEntity>
        where D : class
    {
    }
}

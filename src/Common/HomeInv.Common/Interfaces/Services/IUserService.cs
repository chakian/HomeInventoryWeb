using HomeInv.Common.ServiceContracts.User;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IUserService<D> : IUserService, IServiceBase
        where D : class
    {
    }

    public interface IUserService : IServiceBase
    {
        SearchUserResponse SearchUser(SearchUserRequest request);
    }
}

using HomeInv.Common.ServiceContracts.HomeUser;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IHomeUserService : IServiceBase
    {
        InsertHomeUserResponse InsertHomeUser(InsertHomeUserRequest request);
    }
}

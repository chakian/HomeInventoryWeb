using HomeInv.Common.ServiceContracts.AreaUser;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IAreaUserService<D> : IAreaUserService, IServiceBase<D, Entities.AreaUserEntity>
        where D : class
    {
    }

    public interface IAreaUserService : IServiceBase
    {
        InsertAreaUserResponse InsertAreaUser(InsertAreaUserRequest request);
    }
}

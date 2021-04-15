namespace HomeInv.Common.Interfaces.Services
{
    public interface IHomeUserService : IServiceBase
    {
        bool InsertHomeUser(int homeId, string userId, string role);
    }
}

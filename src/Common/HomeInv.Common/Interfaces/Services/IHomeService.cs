using HomeInv.Common.ServiceContracts.Home;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IHomeService : IServiceBase
    {
        //HomeEntity GetHomeById(int id);
        CreateHomeResponse CreateHome(CreateHomeRequest request);
        GetHomesOfUserResponse GetHomesOfUser(GetHomesOfUserRequest request);
    }
}

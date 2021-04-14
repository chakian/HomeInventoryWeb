using HomeInv.Common.Entities;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IHomeService : IServiceBase
    {
        //HomeEntity GetHomeById(int id);
        //IQueryable<HomeEntity> GetHomesOfUser(string userId);
        HomeEntity CreateHome(HomeEntity homeEntity, string userId);
    }
}

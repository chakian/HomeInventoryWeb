using HomeInv.Common.Entities;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IHomeService : IServiceBase
    {
        //HomeEntity GetHomeById(int id);
        HomeEntity CreateHome(HomeEntity homeEntity, string userId);
        List<HomeEntity> GetHomesOfUser(string userId);
    }
}

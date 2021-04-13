using HomeInv.Common.Entities;
using HomeInv.Common.ServiceContracts;
using System.Threading.Tasks;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IUserService
    {
        LoginUserResponse LogIn(LoginUserRequest user);

        Task<RegisterUserResponse> Register(RegisterUserRequest user);
        
        Task LogOut(HIUser user);
    }
}

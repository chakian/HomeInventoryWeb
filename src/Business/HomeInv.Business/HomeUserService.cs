using HomeInv.Business.Base;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.HomeUser;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System.Linq;

namespace HomeInv.Business
{
    public class HomeUserService : ServiceBase, IHomeUserService
    {
        public HomeUserService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public InsertHomeUserResponse InsertHomeUser(InsertHomeUserRequest request)
        {
            InsertHomeUserResponse response = new InsertHomeUserResponse();
            if (context.HomeUsers.Any(q => q.HomeId == request.HomeId && q.UserId == request.UserId))
            {
                response.AddError("Bu kullanıcı zaten bu evde mevcut");
                return response;
            }

            HomeUser homeUser = CreateNewAuditableObject<HomeUser>(request);
            homeUser.HomeId = request.HomeId;
            homeUser.UserId = request.UserId;
            homeUser.Role = request.Role;

            context.HomeUsers.Add(homeUser);
            context.SaveChanges();

            return response;
        }
    }
}

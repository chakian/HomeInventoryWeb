using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.HomeUser;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System.Linq;

namespace HomeInv.Business
{
    public class HomeUserService : AuditableServiceBase<HomeUser, HomeUserEntity>, IHomeUserService<HomeUser>
    {
        public HomeUserService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override HomeUserEntity ConvertDboToEntity(HomeUser dbo)
        {
            throw new System.NotImplementedException();
        }

        public InsertHomeUserResponse InsertHomeUser(InsertHomeUserRequest request)
        {
            InsertHomeUserResponse response = new InsertHomeUserResponse();
            if (context.HomeUsers.Any(q => q.HomeId == request.HomeId && q.UserId == request.UserId))
            {
                response.AddError(Resources.UserIsAlreadyInThatHome);
                return response;
            }
            else if (context.HomeUsers.Any(q => q.UserId == request.UserId))
            {
                response.AddError(Resources.UserAlreadyHasAHome);
                return response;
            }

            HomeUser homeUser = CreateNewAuditableObject(request);
            homeUser.HomeId = request.HomeId;
            homeUser.UserId = request.UserId;
            homeUser.Role = request.Role;

            context.HomeUsers.Add(homeUser);
            context.SaveChanges();

            return response;
        }
    }
}

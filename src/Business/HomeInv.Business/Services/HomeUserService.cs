using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.HomeUser;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business.Services
{
    public class HomeUserService : AuditableServiceBase<HomeUser, HomeUserEntity>, IHomeUserService<HomeUser>
    {
        public HomeUserService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override HomeUserEntity ConvertDboToEntity(HomeUser dbo)
        {
            return new HomeUserEntity()
            {
                Id = dbo.Id,
                HomeId = dbo.HomeId,
                UserId = dbo.UserId,
                Username = dbo.User.UserName,
                Role = dbo.Role
            };
        }

        public GetUsersOfHomeResponse GetUsersOfHome(GetUsersOfHomeRequest request)
        {
            var response = new GetUsersOfHomeResponse();

            var homeUsers = context.HomeUsers
                .Where(homeUser => homeUser.HomeId == request.HomeId)
                .Include(user => user.User).ToList();
            var homeUserEntities = new List<HomeUserEntity>();
            foreach (var homeUser in homeUsers)
            {
                homeUserEntities.Add(ConvertDboToEntity(homeUser));
            }

            response.HomeUsers = homeUserEntities;

            return response;
        }

        public InsertHomeUserResponse InsertHomeUser(InsertHomeUserRequest request)
        {
            var response = new InsertHomeUserResponse();
            if (context.HomeUsers.Any(q => q.HomeId == request.HomeId && q.UserId == request.UserId))
            {
                response.AddError(Resources.UserIsAlreadyInThatHome);
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

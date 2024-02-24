using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.HomeUser;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<GetUsersOfHomeResponse> GetUsersOfHomeAsync(GetUsersOfHomeRequest request, CancellationToken ct)
        {
            var response = new GetUsersOfHomeResponse();

            var homeUsers = await context.HomeUsers
                .Where(homeUser => homeUser.HomeId == request.HomeId)
                .Include(user => user.User).ToListAsync(ct);
            var homeUserEntities = new List<HomeUserEntity>();
            foreach (var homeUser in homeUsers)
            {
                homeUserEntities.Add(ConvertDboToEntity(homeUser));
            }

            response.HomeUsers = homeUserEntities;

            return response;
        }

        public async Task<InsertHomeUserResponse> InsertHomeUserAsync(InsertHomeUserRequest request, CancellationToken ct)
        {
            var response = new InsertHomeUserResponse();
            if (await context.HomeUsers.AnyAsync(q => q.HomeId == request.HomeId && q.UserId == request.UserId, ct))
            {
                response.AddError(Resources.UserIsAlreadyInThatHome);
                return response;
            }

            var homeUser = CreateNewAuditableObject(request);
            homeUser.HomeId = request.HomeId;
            homeUser.UserId = request.UserId;
            homeUser.Role = request.Role;

            context.HomeUsers.Add(homeUser);
            await context.SaveChangesAsync(ct);

            return response;
        }
    }
}

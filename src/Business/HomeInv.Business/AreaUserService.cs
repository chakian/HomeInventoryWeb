using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.AreaUser;
using HomeInv.Common.ServiceContracts.HomeUser;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeInv.Business
{
    public class AreaUserService : AuditableServiceBase<AreaUser, AreaUserEntity>, IAreaUserService<AreaUser>
    {
        public AreaUserService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override AreaUserEntity ConvertDboToEntity(AreaUser dbo)
        {
            return new AreaUserEntity()
            {
                Id = dbo.Id,
                AreaId = dbo.AreaId,
                UserId = dbo.UserId,
                Username = dbo.User.UserName,
                Role = dbo.Role
            };
        }

        public InsertAreaUserResponse InsertAreaUser(InsertAreaUserRequest request)
        {
            var response = new InsertAreaUserResponse();
            if (context.AreaUsers.Any(q => q.AreaId == request.AreaId && q.UserId == request.UserId))
            {
                response.AddError("Bu alan için bu kullanıcı zaten yetkiye sahip.");
                return response;
            }

            AreaUser areaUser = CreateNewAuditableObject(request);
            areaUser.AreaId = request.AreaId;
            areaUser.UserId = request.UserId;
            areaUser.Role = request.Role;

            context.AreaUsers.Add(areaUser);
            context.SaveChanges();

            return response;
        }
    }
}

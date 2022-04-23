using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.User;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business
{
    public class UserService : ServiceBase, IUserService<UserEntity>
    {
        public UserService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public UserEntity ConvertDboToEntity(User dbo)
        {
            return new UserEntity()
            {
                UserId = dbo.Id,
                Username = dbo.UserName
            };
        }

        public SearchUserResponse SearchUser(SearchUserRequest request)
        {
            var response = new SearchUserResponse();

            var dbResult = context.Users.Where(user => user.UserName.Contains(request.SearchQuery)).ToList();
            var results = new List<UserEntity>();

            foreach (var user in dbResult)
            {
                results.Add(ConvertDboToEntity(user));
            }

            response.SearchResults = results;

            return response;
        }
    }
}

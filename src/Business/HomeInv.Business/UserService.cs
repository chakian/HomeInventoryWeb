using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts;
using HomeInv.Common.Settings;
using HomeInv.Persistence.Dbo;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace HomeInv.Business
{
    public class UserService : ServiceBase, IUserService
    {
        public UserService(IOptions<CosmosSettings> _cosmosSettings)// : base(_cosmosSettings)
        {
        }

        public override string GetCollectionName() => "Users";

        public LoginUserResponse LogIn(LoginUserRequest request)
        {
            //var foundUser = GetDocumentByPredicate<User>(u => u.Email == request.User.Email);

            LoginUserResponse response = new LoginUserResponse();

            //string passwordHash = Common.Utils.Hashing.GetHash(request.User.Password);
            //if (foundUser != null && foundUser.PasswordHash == passwordHash)
            //{
            //    response.User = new HIUser()
            //    {
            //        Email = foundUser.Email,
            //        Id = foundUser.id
            //    };
            //}
            //else
            //{
            //    response.AddError("Mail adresi veya şifre hatalı.");
            //}

            return response;
        }

        public Task LogOut(HIUser user) => throw new NotImplementedException();

        public async Task<RegisterUserResponse> Register(RegisterUserRequest request)
        {
            //UserDbo userDbo = new UserDbo();
            //userDbo.Email = request.User.Email;
            //userDbo.PasswordHash = Common.Utils.Hashing.GetHash(request.User.Password);
            //userDbo = await CreateDocument<UserDbo>(userDbo);

            RegisterUserResponse response = new RegisterUserResponse()
            {
                //User = new HIUser()
                //{
                //    Id = userDbo.id,
                //    Email = userDbo.Email
                //}
            };

            return response;
        }
    }
}

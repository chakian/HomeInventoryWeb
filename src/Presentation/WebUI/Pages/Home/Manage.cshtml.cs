using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Common.ServiceContracts.HomeUser;
using HomeInv.Common.ServiceContracts.User;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebUI.Base;

namespace WebUI.Pages.Home
{
    public class ManageModel : BaseAuthenticatedPageModel<ManageModel>
    {
        readonly IHomeService homeService;
        readonly IHomeUserService homeUserService;
        readonly IUserService userService;

        public ManageModel(ILogger<ManageModel> logger,
            HomeInventoryDbContext dbContext,
            IHomeService homeService,
            IHomeUserService homeUserService,
            IUserService userService) : base(logger, dbContext)
        {
            this.homeService = homeService;
            this.homeUserService = homeUserService;
            this.userService = userService;
        }

        [BindProperty]
        public HomeEntity Home { get; set; }

        [BindProperty]
        public List<HomeUserEntity> HomeUsers { get; set; }

        [BindProperty]
        public string SearchQuery { get; set; }

        [BindProperty]
        public List<SelectableUserResult> UserResult { get; set; }

        public class SelectableUserResult
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool IsSelected { get; set; }
        }

        public void OnGet(int homeId, string searchQuery)
        {
            var request = new GetSingleHomeOfUserRequest
            {
                HomeId = homeId,
                RequestUserId = UserId,
            };
            var homeResponse = homeService.GetSingleHomeOfUser(request);
            Home = homeResponse.Home;

            var getUsersOfHomeRequest = new GetUsersOfHomeRequest
            {
                HomeId = homeId,
                RequestUserId = UserId,
            };
            var homeUserResponse = homeUserService.GetUsersOfHome(getUsersOfHomeRequest);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var searchUserRequest = new SearchUserRequest { SearchQuery = searchQuery, RequestUserId = UserId };
                var searchUserResponse = userService.SearchUser(searchUserRequest);
                UserResult = searchUserResponse.SearchResults.Select(res => new SelectableUserResult()
                {
                    Id = res.UserId,
                    Name = res.Username,
                    IsSelected = false
                }).ToList();

                SearchQuery = searchQuery;
            }

            HomeUsers = homeUserResponse.HomeUsers;
        }

        protected override IActionResult OnModelPost()
        {
            if(UserResult != null && UserResult.Any(result => result.IsSelected))
            {
                foreach (var user in UserResult.Where(result => result.IsSelected))
                {
                    var insertRequest = new InsertHomeUserRequest()
                    {
                        UserId = user.Id,
                        HomeId = Home.Id,
                        RequestUserId = UserId,
                        Role = "owner"
                    };
                    CallService(homeUserService.InsertHomeUser, insertRequest);
                }
            }

            var routeValues = new
            {
                homeId = Home.Id,
                searchQuery = SearchQuery,
            };
            return RedirectToPage("/Home/Manage", routeValues);
        }

        public class SelectableUser
        {
            public string Id { get; set; }
            public int IsSelected { get; set; }
        }
    }
}

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
        public List<UserEntity> UserResult { get; set; }

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
                UserResult = searchUserResponse.SearchResults;

                SearchQuery = searchQuery;
            }

            HomeUsers = homeUserResponse.HomeUsers;
        }

        protected override IActionResult OnModelPost()
        {
            

            var routeValues = new
            {
                homeId = Home.Id,
                searchQuery = SearchQuery,
            };
            return RedirectToPage("/Manage", routeValues);
        }

        public class SelectableUser
        {
            public string Id { get; set; }
            public int IsSelected { get; set; }
        }
    }
}

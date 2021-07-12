using System.Collections.Generic;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Base;

namespace WebUI.Pages.Home
{
    public class ListModel : BasePageModel<ListModel>
    {
        readonly IHomeService homeService;
        public ListModel(ILogger<ListModel> logger, HomeInventoryDbContext dbContext, IHomeService homeService) : base(logger, dbContext)
        {
            this.homeService = homeService;
        }

        [BindProperty]
        public List<HomeEntity> Homes { get; set; }

        public IActionResult OnGet()
        {
            var request = new GetHomesOfUserRequest()
            {
                RequestUserId = UserId
            };
            var homesResponse = homeService.GetHomesOfUser(request);
            if (homesResponse.IsSuccessful)
            {
                Homes = homesResponse.Homes;
            }
            return Page();
        }
    }
}

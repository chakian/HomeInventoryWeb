using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Language;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Base;

namespace WebUI.Pages.Home
{
    public class EditModel : BaseAuthenticatedPageModel<EditModel>
    {
        readonly IHomeService homeService;

        public EditModel(ILogger<EditModel> logger,
            HomeInventoryDbContext dbContext,
            IHomeService homeService) : base(logger, dbContext)
        {
            this.homeService = homeService;
        }

        [BindProperty]
        public HomeEntity Home { get; set; }

        public void OnGet(int homeId)
        {
            var request = new GetSingleHomeOfUserRequest
            {
                HomeId = homeId,
                RequestUserId = UserId,
            };
            var homeResponse = homeService.GetSingleHomeOfUser(request);
            Home = homeResponse.Home;
        }

        protected override IActionResult OnModelPost()
        {
            var updateHomeRequest = new UpdateHomeRequest()
            {
                HomeEntity = Home,
                RequestUserId = UserId
            };
            var response = CallService(homeService.UpdateHome, updateHomeRequest, Resources.Success_Home_Edit);

            if (response.IsSuccessful) return RedirectToPage("/Home/List");
            return Page();
        }
    }
}

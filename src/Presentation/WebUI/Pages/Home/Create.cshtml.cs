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
    public class CreateModel : BaseAuthenticatedPageModel<CreateModel>
    {
        readonly IHomeService homeService;
        public CreateModel(ILogger<CreateModel> logger, 
            HomeInventoryDbContext dbContext, 
            IHomeService homeService) : base(logger, dbContext)
        {
            this.homeService = homeService;
        }

        [BindProperty]
        public HomeEntity Home { get; set; }
        [BindProperty]
        public bool IsFirstHome { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        protected override IActionResult OnModelPost()
        {
            var createHomeRequest = new CreateHomeRequest()
            {
                HomeEntity = Home,
                RequestUserId = UserId
            };
            var response = CallService(homeService.CreateHome, createHomeRequest, Resources.Success_Home_Create);

            if (response.IsSuccessful) return RedirectToPage("/Home/List");
            return Page();
        }
    }
}

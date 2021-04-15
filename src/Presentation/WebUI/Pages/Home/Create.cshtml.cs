using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Base;

namespace WebUI.Pages.Home
{
    public class CreateModel : BasePageModel<CreateModel>
    {
        readonly IHomeService homeService;
        readonly IHomeUserService homeUserService;
        public CreateModel(ILogger<CreateModel> logger, HomeInventoryDbContext dbContext, IHomeService homeService, IHomeUserService homeUserService) : base(logger, dbContext)
        {
            this.homeUserService = homeUserService;
            this.homeService = homeService;
        }

        [BindProperty]
        public HomeEntity Home { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        protected override IActionResult OnModelPost()
        {
            var request = new CreateHomeRequest()
            {
                HomeEntity = Home,
                UserId = UserId
            };
            var home = homeService.CreateHome(request);
            homeUserService.InsertHomeUser(home.HomeEntity.Id, UserId, "owner");

            return RedirectToPage("/Index");
        }
    }
}

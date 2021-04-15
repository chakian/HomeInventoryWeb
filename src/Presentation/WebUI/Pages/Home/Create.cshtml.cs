using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Common.ServiceContracts.HomeUser;
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
            var createHomeRequest = new CreateHomeRequest()
            {
                HomeEntity = Home,
                RequestUserId = UserId
            };
            var home = homeService.CreateHome(createHomeRequest);
            var insertHomeUserRequest = new InsertHomeUserRequest()
            {
                HomeId = home.HomeEntity.Id,
                UserId = UserId,
                Role = "owner",
                RequestUserId = UserId
            };
            homeUserService.InsertHomeUser(insertHomeUserRequest);

            return RedirectToPage("/Index");
        }
    }
}

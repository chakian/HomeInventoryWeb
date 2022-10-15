using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Area;
using HomeInv.Common.ServiceContracts.AreaUser;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Language;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WebUI.Base;

namespace WebUI.Pages.Home
{
    public class CreateModel : BaseAuthenticatedPageModel<CreateModel>
    {
        readonly IHomeService homeService;
        readonly IAreaService areaService;
        readonly IAreaUserService areaUserService;
        public CreateModel(ILogger<CreateModel> logger, 
            HomeInventoryDbContext dbContext, 
            IHomeService homeService, 
            IAreaService areaService,
            IAreaUserService areaUserService) : base(logger, dbContext)
        {
            this.homeService = homeService;
            this.areaService = areaService;
            this.areaUserService = areaUserService;
        }

        [BindProperty]
        public HomeEntity Home { get; set; }
        [BindProperty]
        public bool IsFirstHome { get; set; }

        public IActionResult OnGet()
        {
            IsFirstHome = (SelectedHomeId == 0);
            return Page();
        }

        protected override IActionResult OnModelPost()
        {
            var createHomeRequest = new CreateHomeRequest()
            {
                HomeEntity = Home,
                RequestUserId = UserId
            };
            var r = CallService(homeService.CreateHome, createHomeRequest);
            
            return RedirectToPage("/Home/List");
        }
    }
}

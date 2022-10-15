using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Area;
using HomeInv.Common.ServiceContracts.AreaUser;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Common.ServiceContracts.HomeUser;
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
        readonly IHomeUserService homeUserService;
        readonly IAreaService areaService;
        readonly IAreaUserService areaUserService;
        public CreateModel(ILogger<CreateModel> logger, 
            HomeInventoryDbContext dbContext, 
            IHomeService homeService, 
            IHomeUserService homeUserService,
            IAreaService areaService,
            IAreaUserService areaUserService) : base(logger, dbContext)
        {
            this.homeUserService = homeUserService;
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
            var home = homeService.CreateHome(createHomeRequest);
            var insertHomeUserRequest = new InsertHomeUserRequest()
            {
                HomeId = home.HomeEntity.Id,
                UserId = UserId,
                Role = "owner",
                RequestUserId = UserId
            };
            homeUserService.InsertHomeUser(insertHomeUserRequest);

            /// TEMPORARY SOLUTION UNTIL AREAS ARE REALLY IMPLEMENTED
            var createAreaRequest = new CreateAreaRequest()
            {
                AreaEntity = new AreaEntity()
                {
                    HomeId = home.HomeEntity.Id,
                    Name = "Genel"
                },
                RequestUserId = UserId
            };
            var area = areaService.CreateArea(createAreaRequest);
            var insertAreaUserRequest = new InsertAreaUserRequest()
            {
                AreaId = area.AreaEntity.Id,
                UserId = UserId,
                Role = "owner",
                RequestUserId = UserId
            };
            areaUserService.InsertAreaUser(insertAreaUserRequest);
            /// TEMPORARY SOLUTION UNTIL AREAS ARE REALLY IMPLEMENTED
            
            SetSuccessMessage(Resources.Success_Home_Create);

            return RedirectToPage("/Home/List");
        }
    }
}

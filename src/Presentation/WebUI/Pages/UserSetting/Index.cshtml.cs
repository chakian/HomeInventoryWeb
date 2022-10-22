using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Common.ServiceContracts.UserSetting;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebUI.Base;

namespace WebUI.Pages.UserSetting
{
    public class IndexModel : BaseAuthenticatedPageModel<IndexModel>
    {
        readonly IUserSettingService userSettingService;
        readonly IHomeService homeService;

        public IndexModel(ILogger<IndexModel> logger,
            HomeInventoryDbContext dbContext,
            IUserSettingService userSettingService,
            IHomeService homeService) : base(logger, dbContext)
        {
            this.userSettingService = userSettingService;
            this.homeService = homeService;
        }

        [BindProperty]
        public UserSettingEntity UserSetting { get; set; }
        [BindProperty]
        public List<HomeEntity> HomeList { get; set; }
        [BindProperty]
        public int SelectedHomeId { get; set; }

        public void OnGet(int homeId)
        {
            InitiatePageModel();
        }

        private void InitiatePageModel()
        {
            var request = new GetUserSettingsRequest
            {
                UserId = UserId,
                RequestUserId = UserId,
            };
            var settings = userSettingService.GetUserSettings(request);
            UserSetting = settings.UserSetting;

            SelectedHomeId = (UserSetting != null) ? UserSetting.DefaultHomeId : 0;

            var homeRequest = new GetHomesOfUserRequest()
            {
                RequestUserId = UserId
            };
            var homesResponse = homeService.GetHomesOfUser(homeRequest);
            HomeList = homesResponse.Homes;
        }

        protected override IActionResult OnModelPost()
        {
            var updateUserSettingsRequest = new UpdateUserSettingsRequest()
            {
                UserSettingEntity = UserSetting,
                RequestUserId = UserId
            };
            var response = CallService(userSettingService.UpdateUserSettings, updateUserSettingsRequest);

            if (response.IsSuccessful) HttpContext.Session.Clear();

            InitiatePageModel();

            return Page();
        }
    }
}

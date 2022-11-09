using System.Collections.Generic;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Base;

namespace WebUI.Pages.Item
{
    public class ListModel : BaseAuthenticatedPageModel<ListModel>
    {
        readonly IItemDefinitionService itemService;
        public ListModel(ILogger<ListModel> logger, HomeInventoryDbContext dbContext, IItemDefinitionService itemService) : base(logger, dbContext)
        {
            this.itemService = itemService;
        }

        [BindProperty]
        public List<ItemDefinitionEntity> Items { get; set; }

        public IActionResult OnGet()
        {
            var request = new GetAllItemDefinitionsInHomeRequest()
            {
                HomeId = UserSettings.DefaultHomeId,
                RequestUserId = UserId
            };
            var itemsResponse = itemService.GetAllItemDefinitionsInHome(request);
            if (itemsResponse.IsSuccessful)
            {
                Items = itemsResponse.Items;
            }
            return Page();
        }
    }
}

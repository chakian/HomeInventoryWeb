using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebUI.Base;

namespace WebUI.Pages.ItemDefinition
{
    public class ListModel : BaseAuthenticatedPageModel<ListModel>
    {
        readonly IItemDefinitionService itemDefinitionService;
        public ListModel(ILogger<ListModel> logger, HomeInventoryDbContext dbContext, IItemDefinitionService itemDefinitionService) : base(logger, dbContext)
        {
            this.itemDefinitionService = itemDefinitionService;
        }

        [BindProperty]
        public List<ItemDefinitionEntity> ItemDefinitions { get; set; }

        public void OnGet()
        {
            var request = new GetAllItemDefinitionsInHomeRequest()
            {
                HomeId = UserSettings.DefaultHomeId,
                RequestUserId = UserId
            };
            var itemsResponse = itemDefinitionService.GetAllItemDefinitionsInHome(request, true);

            if (!itemsResponse.IsSuccessful)
            {
                SetErrorMessage(itemsResponse.Result.ToString());
            }
            else
            {
                ItemDefinitions = itemsResponse.Items;
            }
        }
    }
}

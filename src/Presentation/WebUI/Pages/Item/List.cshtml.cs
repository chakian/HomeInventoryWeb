using System.Collections.Generic;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Item;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Base;

namespace WebUI.Pages.Item
{
    public class ListModel : BaseAuthenticatedPageModel<ListModel>
    {
        readonly IItemService itemService;
        public ListModel(ILogger<ListModel> logger, HomeInventoryDbContext dbContext, IItemService itemService) : base(logger, dbContext)
        {
            this.itemService = itemService;
        }

        [BindProperty]
        public List<ItemEntity> Items { get; set; }

        public IActionResult OnGet()
        {
            var request = new GetAllItemsInHomeRequest()
            {
                HomeId = SelectedHomeId,
                RequestUserId = UserId
            };
            var itemsResponse = itemService.GetAllItemsInHome(request);
            if (itemsResponse.IsSuccessful)
            {
                Items = itemsResponse.Items;
            }
            return Page();
        }
    }
}

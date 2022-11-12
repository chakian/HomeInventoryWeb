using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Language;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebUI.Base;

namespace WebUI.Pages.Stock
{
    public class CreateStep1Model : BaseAuthenticatedPageModel<CreateStep1Model>
    {
        readonly IItemDefinitionService _itemDefinitionService;

        public CreateStep1Model(ILogger<CreateStep1Model> logger, 
            HomeInventoryDbContext dbContext,
            IItemDefinitionService itemDefinitionService) : base(logger, dbContext)
        {
            _itemDefinitionService = itemDefinitionService;
        }

        [BindProperty]
        public List<SelectListItem> AllItemDefinitions { get; set; }

        [BindProperty]
        [Display(Name = nameof(Resources.ItemStockEntry_ItemDefinitionId), ResourceType = typeof(Resources))]
        [Required]
        public int ItemDefinitionId { get; set; }

        public void OnGet()
        {
            AllItemDefinitions = new List<SelectListItem>()
            {
                new SelectListItem { Text = "-- Urun --", Value = "0", Selected = true }
            };
            var definitionList = _itemDefinitionService.GetAllItemDefinitionsInHome(new GetAllItemDefinitionsInHomeRequest()
            {
                HomeId = UserSettings.DefaultHomeId,
                RequestUserId = UserId
            });

            foreach ( var definition in definitionList.Items )
            {
                AllItemDefinitions.Add(new SelectListItem()
                {
                    Text = definition.Name,
                    Value = definition.Id.ToString()
                });
            }
        }

        public override IActionResult OnPost()
        {
            if (ItemDefinitionId > 0)
            {
                var routeValues = new
                {
                    ItemDefinitionId,
                };
                return RedirectToPage("CreateStep2", routeValues);
            }
            else
            {
                OnGet();
                SetErrorMessage("Urun secimi yapmadan bu adimi gecemezsiniz ama");
                return Page();
            }
        }
    }
}

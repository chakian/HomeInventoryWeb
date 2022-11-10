using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Common.ServiceContracts.SizeUnit;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebUI.Base;

namespace WebUI.Pages.ItemDefinition
{
    public class CreateModel : BaseAuthenticatedPageModel<CreateModel>
    {
        readonly IItemDefinitionService _itemDefinitionService;
        readonly ICategoryService _categoryService;
        public CreateModel(ILogger<CreateModel> logger,
            HomeInventoryDbContext dbContext,
            IItemDefinitionService itemDefinitionService,
            ICategoryService categoryService) : base(logger, dbContext)
        {
            _itemDefinitionService = itemDefinitionService;
            _categoryService = categoryService;
        }

        [BindProperty] public ItemDefinitionEntity Item { get; set; }

        [BindProperty] public List<SelectListItem> AllCategories { get; set; }

        public IActionResult OnGet()
        {
            var categoryList = _categoryService.GetCategoriesOfHome_Ordered(new GetCategoriesOfHomeRequest()
            {
                HomeId = UserSettings.DefaultHomeId
            }).Categories;

            AllCategories = new List<SelectListItem>
            {
                new SelectListItem("-- Kategori --", "")
            };
            categoryList.ForEach(category =>
            {
                AllCategories.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            });

            return Page();
        }

        protected override IActionResult OnModelPost()
        {
            var createRequest = new CreateItemDefinitionRequest()
            {
                ItemEntity = Item,
                HomeId = UserSettings.DefaultHomeId,
                RequestUserId = UserId
            };
            var response = CallService(_itemDefinitionService.CreateItemDefinition, createRequest);
            
            return RedirectToPage("List");
        }
    }
}

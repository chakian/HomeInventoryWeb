using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Common.ServiceContracts.SizeUnit;
using HomeInv.Language;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Xml.Linq;
using WebUI.Base;

namespace WebUI.Pages.ItemDefinition
{
    public class CreateModel : BaseAuthenticatedPageModel<CreateModel>
    {
        readonly IItemDefinitionService _itemDefinitionService;
        readonly ICategoryService _categoryService;
        readonly ISizeUnitService _sizeUnitService;

        public CreateModel(ILogger<CreateModel> logger,
            HomeInventoryDbContext dbContext,
            IItemDefinitionService itemDefinitionService,
            ICategoryService categoryService,
            ISizeUnitService sizeUnitService) : base(logger, dbContext)
        {
            _itemDefinitionService = itemDefinitionService;
            _categoryService = categoryService;
            _sizeUnitService = sizeUnitService;
        }

        [BindProperty] public ItemDefinitionEntity Item { get; set; }

        [BindProperty] public List<SelectListItem> AllCategories { get; set; }
        [BindProperty] public List<SelectListItem> AllSizeUnits { get; set; }

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

            AllSizeUnits = new List<SelectListItem>() { new SelectListItem() { Text = "-- Boyut --", Value = "0" } };
            var sizes = _sizeUnitService.GetAllSizes(new GetAllSizesRequest() { RequestUserId = UserId });
            foreach (var size in sizes.SizeUnits)
            {
                AllSizeUnits.Add(new SelectListItem()
                {
                    Text = size.Name,
                    Value = size.Id.ToString()
                });
            }

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

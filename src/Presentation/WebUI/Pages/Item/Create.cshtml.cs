using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Common.ServiceContracts.SizeUnit;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebUI.Base;

namespace WebUI.Pages.Item
{
    public class CreateModel : BaseAuthenticatedPageModel<CreateModel>
    {
        readonly ICategoryService categoryService;
        readonly ISizeUnitService sizeUnitService;
        public CreateModel(ILogger<CreateModel> logger,
            HomeInventoryDbContext dbContext,
            ICategoryService categoryService,
            ISizeUnitService sizeUnitService) : base(logger, dbContext)
        {
            this.categoryService = categoryService;
            this.sizeUnitService = sizeUnitService;
        }

        [BindProperty] public ItemEntity Item { get; set; }

        [BindProperty] public List<SelectListItem> AllCategories { get; set; }

        [BindProperty] public List<SelectListItem> AllSizeUnits { get; set; }

        public IActionResult OnGet()
        {
            var categoryList = categoryService.GetCategoriesOfHome_Ordered(new GetCategoriesOfHomeRequest()
            {
                HomeId = SelectedHomeId
            }).Categories;
            AllCategories = new List<SelectListItem>();
            AllCategories.Add(new SelectListItem("Üst Kategori", ""));
            categoryList.ForEach(category =>
            {
                AllCategories.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                });
            });

            var sizeUnitList = sizeUnitService.GetAllSizes(new GetAllSizesRequest()
            {
                HomeId = SelectedHomeId,
                RequestUserId = UserId
            }).SizeUnits;
            AllSizeUnits = new List<SelectListItem>();
            AllSizeUnits.Add(new SelectListItem("Üst Kategori", ""));
            sizeUnitList.ForEach(sizeUnit =>
            {
                AllSizeUnits.Add(new SelectListItem()
                {
                    Text = sizeUnit.Name,
                    Value = sizeUnit.Id.ToString()
                });
            });
            return Page();
        }

        protected override IActionResult OnModelPost()
        {
            //var createCategoryRequest = new CreateCategoryRequest()
            //{
            //    CategoryEntity = Category,
            //    HomeId = SelectedHomeId,
            //    RequestUserId = UserId
            //};
            //var category = categoryService.CreateCategory(createCategoryRequest);

            return RedirectToPage("List");
        }
    }
}

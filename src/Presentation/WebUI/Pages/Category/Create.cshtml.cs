using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Language;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebUI.Base;

namespace WebUI.Pages.Category
{
    public class CreateModel : BaseAuthenticatedPageModel<CreateModel>
    {
        readonly ICategoryService categoryService;
        public CreateModel(ILogger<CreateModel> logger, 
            HomeInventoryDbContext dbContext,
            ICategoryService categoryService) : base(logger, dbContext)
        {
            this.categoryService = categoryService;
        }

        [BindProperty]public CategoryEntity Category { get; set; }
        [BindProperty]public List<SelectListItem> AllCategories { get; set; }

        public IActionResult OnGet()
        {
            var categoryList = categoryService.GetCategoriesOfHome_Ordered(new GetCategoriesOfHomeRequest()
            {
                HomeId = SelectedHomeId
            }).Categories;
            AllCategories = new List<SelectListItem>();
            AllCategories.Add(new SelectListItem("�st Kategori", ""));
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
            var createCategoryRequest = new CreateCategoryRequest()
            {
                CategoryEntity = Category,
                HomeId = SelectedHomeId,
                RequestUserId = UserId
            };
            var categoryResponse = CallService(categoryService.CreateCategory, createCategoryRequest, Resources.Success_Category_Create);

            if (categoryResponse.IsSuccessful)
            {
                return RedirectToPage("List");
            }
            else
            {
                return RedirectToPage("/Category/Create");
            }
        }
    }
}

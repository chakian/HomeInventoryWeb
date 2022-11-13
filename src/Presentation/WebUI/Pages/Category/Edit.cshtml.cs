using HomeInv.Business.Services;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Language;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebUI.Base;

namespace WebUI.Pages.Category
{
    public class EditModel : BaseAuthenticatedPageModel<EditModel>
    {
        readonly ICategoryService _categoryService;

        public EditModel(ILogger<EditModel> logger, 
            HomeInventoryDbContext dbContext,
            ICategoryService categoryService) : base(logger, dbContext)
        {
            _categoryService = categoryService;
        }

        [BindProperty] public CategoryEntity Category { get; set; }
        [BindProperty] public List<SelectListItem> AllCategories { get; set; }

        public IActionResult OnGet()
        {
            if (!int.TryParse(Request.Query["Id"].ToString(), out int _categoryId))
            {
                SetErrorMessage("Oyle bir kategori bulamadik. Butona dikkatli tiklayiniz.");
                return RedirectToPage("List");
            }

            var categoryList = _categoryService.GetCategoriesOfHome_Ordered(new GetCategoriesOfHomeRequest()
            {
                HomeId = UserSettings.DefaultHomeId
            }).Categories;

            Category = categoryList.SingleOrDefault(cat => cat.Id == _categoryId);

            if (Category == null)
            {
                SetErrorMessage("Oyle bir kategori bulamadik. Butona dikkatli tiklayiniz.");
                return RedirectToPage("List");
            }

            AllCategories = new List<SelectListItem> { new SelectListItem("Üst Kategori", "") };
            categoryList.ForEach(category =>
            {
                AllCategories.Add(new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id.ToString(),
                    Selected = (Category.ParentCategoryId == category.Id)
                });
            });

            return Page();
        }

        protected override IActionResult OnModelPost()
        {
            var updateCategoryRequest = new UpdateCategoryRequest()
            {
                CategoryId = Category.Id,
                Name = Category.Name,
                Description = Category.Description,
                ParentCategoryId = Category.ParentCategoryId,
                RequestUserId = UserId
            };
            var categoryResponse = CallService(_categoryService.UpdateCategory, updateCategoryRequest);

            if (categoryResponse.IsSuccessful)
            {
                return RedirectToPage("List");
            }
            else
            {
                return RedirectToPage("/Category/Edit?Id=" + Category.Id);
            }
        }
    }
}

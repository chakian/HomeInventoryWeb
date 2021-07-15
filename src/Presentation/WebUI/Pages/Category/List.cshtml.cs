using System.Collections.Generic;
using HomeInv.Common.Constants;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUI.Base;

namespace WebUI.Pages.Category
{
    public class ListModel : BaseAuthenticatedPageModel<ListModel>
    {
        readonly ICategoryService categoryService;
        public ListModel(ILogger<ListModel> logger, HomeInventoryDbContext dbContext, ICategoryService categoryService) : base(logger, dbContext)
        {
            this.categoryService = categoryService;
        }

        [BindProperty]
        public List<CategoryEntity> Categories { get; set; }

        public IActionResult OnGet()
        {
            int homeId = HttpContext.Session.GetInt32(SessionKeys.DEFAULT_HOME_ID) ?? 0;
            var request = new GetCategoriesOfHomeRequest()
            {
                HomeId = homeId,
                RequestUserId = UserId
            };
            var categoriesResponse = categoryService.GetCategoriesOfHome_Hierarchial(request);
            if (categoriesResponse.IsSuccessful)
            {
                Categories = categoriesResponse.Categories;
            }
            return Page();
        }
    }
}

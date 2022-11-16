using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Common.ServiceContracts.SizeUnit;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using WebUI.Base;

namespace WebUI.Pages.ItemDefinition
{
    public class CreateModel : BaseAuthenticatedPageModel<CreateModel>
    {
        readonly IWebHostEnvironment _webHostEnvironment;
        readonly IItemDefinitionService _itemDefinitionService;
        readonly ICategoryService _categoryService;
        readonly ISizeUnitService _sizeUnitService;

        public CreateModel(ILogger<CreateModel> logger,
            HomeInventoryDbContext dbContext,
            IWebHostEnvironment webHostEnvironment,
            IItemDefinitionService itemDefinitionService,
            ICategoryService categoryService,
            ISizeUnitService sizeUnitService) : base(logger, dbContext)
        {
            _itemDefinitionService = itemDefinitionService;
            _categoryService = categoryService;
            _sizeUnitService = sizeUnitService;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty] public ItemDefinitionEntity Item { get; set; }

        [BindProperty] public List<SelectListItem> AllCategories { get; set; }
        [BindProperty] public List<SelectListItem> AllSizeUnits { get; set; }
        [BindProperty] public IFormFile FormFile { get; set; }

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
            string imageFileExtension = "";
            if (FormFile != null && !string.IsNullOrEmpty(FormFile.FileName))// && FormFile.FileName.IndexOf(".") > 0)
            {
                imageFileExtension = FormFile.FileName.Substring(FormFile.FileName.LastIndexOf("."));
            }

            var createRequest = new CreateItemDefinitionRequest()
            {
                ItemEntity = Item,
                ImageFileExtension = imageFileExtension,
                HomeId = UserSettings.DefaultHomeId,
                RequestUserId = UserId
            };
            var response = CallService(_itemDefinitionService.CreateItemDefinition, createRequest);

            if (response.IsSuccessful)
            {
                if (FormFile?.Length > 0)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath,
                        "uploads",
                        UserSettings.DefaultHomeId.ToString());
                    Directory.CreateDirectory(path);

                    path = Path.Combine(path, response.ImageFileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        FormFile.CopyTo(stream);
                    }
                }
            }

            return RedirectToPage("List");
        }
    }
}

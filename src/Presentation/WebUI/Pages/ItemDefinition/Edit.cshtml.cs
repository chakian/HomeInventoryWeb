using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Common.ServiceContracts.ItemDefinition;
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
    public class EditModel : BaseAuthenticatedPageModel<EditModel>
    {
        readonly IWebHostEnvironment _webHostEnvironment;
        readonly IItemDefinitionService _itemDefinitionService;
        readonly ICategoryService _categoryService;

        public EditModel(ILogger<EditModel> logger,
            HomeInventoryDbContext dbContext,
            IWebHostEnvironment webHostEnvironment,
            IItemDefinitionService itemDefinitionService,
            ICategoryService categoryService) : base(logger, dbContext)
        {
            _itemDefinitionService = itemDefinitionService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty] public ItemDefinitionEntity Item { get; set; }
        [BindProperty] public List<SelectListItem> AllCategories { get; set; }
        [BindProperty] public IFormFile FormFile { get; set; }
        [BindProperty] public int HomeId { get; set; }

        public IActionResult OnGet()
        {
            if (!int.TryParse(Request.Query["ItemDefinitionId"].ToString(), out int _itemDefinitionId))
            {
                SetErrorMessage("Oyle bir urun bulamadik. Butona dikkatli tiklayiniz.");
                return RedirectToPage("List");
            }

            HomeId = UserSettings.DefaultHomeId;

            var itemDefinition = _itemDefinitionService.GetItemDefinition(new GetItemDefinitionRequest()
            {
                ItemDefinitionId = _itemDefinitionId,
                HomeId = UserSettings.DefaultHomeId,
                RequestUserId = UserId
            });

            Item = itemDefinition.ItemDefinition;

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
                    Value = category.Id.ToString(),
                    Selected = (category.Id == Item.CategoryId)
                });
            });

            return Page();
        }

        protected override IActionResult OnModelPost()
        {
            string imageFileExtension = "", newFileName = "";
            if (FormFile != null && !string.IsNullOrEmpty(FormFile.FileName))
            {
                imageFileExtension = FormFile.FileName.Substring(FormFile.FileName.LastIndexOf("."));
                newFileName = string.Concat("item_", Item.Id, imageFileExtension);
            }

            var updateRequest = new UpdateItemDefinitionRequest()
            {
                ItemDefinitionId = Item.Id,
                Name = Item.Name,
                Description = Item.Description,
                CategoryId = Item.CategoryId,
                ImageFileName = newFileName,
                IsExpirable = Item.IsExpirable,
                HomeId = UserSettings.DefaultHomeId,
                RequestUserId = UserId
            };
            var response = CallService(_itemDefinitionService.UpdateItemDefinition, updateRequest);

            if (response.IsSuccessful)
            {
                if (FormFile?.Length > 0)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath,
                        "uploads",
                        UserSettings.DefaultHomeId.ToString());
                    Directory.CreateDirectory(path);

                    //delete old image if exists
                    if (!string.IsNullOrEmpty(Item.ImageName))
                    {
                        string oldImagePath = Path.Combine(path, Item.ImageName);
                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }

                    path = Path.Combine(path, newFileName);
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

using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business
{
    public class CategoryService : ServiceBase<Category, CategoryEntity>, ICategoryService<Category>
    {
        public CategoryService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override CategoryEntity ConvertDboToEntity(Category dbo)
        {
            throw new NotImplementedException();
        }

        public CreateCategoryResponse CreateCategory(CreateCategoryRequest request)
        {
            var response = new CreateCategoryResponse();

            if(request.HomeId == 0)
            {
                response.AddError(Language.Resources.Category_HomeIsMandatory);
            }
            else
            {
                var existingCategory = context.Categories.Where(category => category.HomeId == request.HomeId && category.Name == request.CategoryName && category.ParentCategoryId == request.ParentCategoryId);
                if(existingCategory != null && existingCategory.Count() > 0)
                {
                    response.AddError(Language.Resources.Category_SameNameExists);
                }
            }

            if (response.IsSuccessful)
            {
                var category = CreateNewObject();
                category.Name = request.CategoryName;
                category.Description = request.Description;
                category.ParentCategoryId = request.ParentCategoryId;
                category.HomeId = request.HomeId;

                context.Categories.Add(category);
                context.SaveChanges();
            }

            return response;
        }

        public GetCategoriesOfHomeResponse GetCategoriesOfHome(GetCategoriesOfHomeRequest request)
        {
            var response = new GetCategoriesOfHomeResponse();

            if (request.HomeId == 0)
            {
                response.AddError(Language.Resources.HomeSelectionIsMandatory);
            }

            if (response.IsSuccessful)
            {
                var dbCategoryList = context.Categories
                    .Where(category => category.HomeId == request.HomeId && category.IsActive)
                    .OrderBy(cat => cat.ParentCategoryId)
                    .ThenBy(cat => cat.Id)
                    .ToList();
                var categoryList = new List<CategoryEntity>();
                foreach (var dbCategory in dbCategoryList)
                {
                    if(!dbCategory.ParentCategoryId.HasValue || dbCategory.ParentCategoryId.Value == 0)
                    {
                        categoryList.Add(new CategoryEntity()
                        {
                            Id = dbCategory.Id,
                            Name = dbCategory.Name,
                            Description = dbCategory.Description,
                            HasParent = false
                        });
                    }
                    else
                    {
                        var parentId = dbCategory.ParentCategoryId.Value;
                        var cat = new CategoryEntity()
                        {
                            Id = dbCategory.Id,
                            Name = dbCategory.Name,
                            Description = dbCategory.Description,
                            HasParent = false
                        };
                        var parent = categoryList.Find(c => c.Id == parentId);
                        parent.Children.Add(cat);
                        parent.HasChild = true;
                    }
                }

                response.Categories = categoryList;
            }

            return response;
        }
    }
}

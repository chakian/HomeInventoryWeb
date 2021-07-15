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
                var existingCategory = context.Categories.Where(category => category.HomeId == request.HomeId && category.Name == request.CategoryEntity.Name && category.ParentCategoryId == request.CategoryEntity.ParentCategoryId);
                if(existingCategory != null && existingCategory.Count() > 0)
                {
                    response.AddError(Language.Resources.Category_SameNameExists);
                }
            }

            if (response.IsSuccessful)
            {
                var category = CreateNewObject();
                category.Name = request.CategoryEntity.Name;
                category.Description = request.CategoryEntity.Description;
                category.ParentCategoryId = request.CategoryEntity.ParentCategoryId;
                category.HomeId = request.HomeId;

                context.Categories.Add(category);
                context.SaveChanges();
            }

            return response;
        }

        private GetCategoriesOfHomeResponse Do_GetCategoriesOfHome_Validation(GetCategoriesOfHomeRequest request)
        {
            var response = new GetCategoriesOfHomeResponse();

            if (request.HomeId == 0)
            {
                response.AddError(Language.Resources.HomeSelectionIsMandatory);
            }

            return response;
        }

        public GetCategoriesOfHomeResponse GetCategoriesOfHome_Hierarchial(GetCategoriesOfHomeRequest request)
        {
            var response = Do_GetCategoriesOfHome_Validation(request);

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
                            HasParent = true,
                            ParentCategoryId = dbCategory.ParentCategoryId
                        };
                        var parent = categoryList.Find(c => c.Id == parentId);
                        if (parent.Children == null) parent.Children = new List<CategoryEntity>();
                        parent.Children.Add(cat);
                        parent.HasChild = true;
                    }
                }

                response.Categories = categoryList;
            }

            return response;
        }

        public GetCategoriesOfHomeResponse GetCategoriesOfHome_Ordered(GetCategoriesOfHomeRequest request)
        {
            var response = Do_GetCategoriesOfHome_Validation(request);

            if (response.IsSuccessful)
            {
                var dbCategoryList = context.Categories
                    .Where(category => category.HomeId == request.HomeId && category.IsActive)
                    .OrderBy(cat => cat.ParentCategoryId)
                    .ThenBy(cat => cat.Id)
                    .ToList();
                var categoryList = OrderCategories(dbCategoryList);

                response.Categories = categoryList;
            }

            return response;
        }

        private List<CategoryEntity> OrderCategories(List<Category> dbCategoryList, List<CategoryEntity> categoryEntities = null, int? parentId = null)
        {
            if (categoryEntities == null) categoryEntities = new List<CategoryEntity>();
            foreach (var dbCategory in dbCategoryList.Where(c => c.ParentCategoryId == parentId))
            {
                categoryEntities.Add(new CategoryEntity()
                {
                    Id = dbCategory.Id,
                    Name = dbCategory.Name,
                    Description = dbCategory.Description,
                    HasParent = dbCategory.ParentCategoryId.HasValue,
                    ParentCategoryId = dbCategory.ParentCategoryId
                });

                if (dbCategoryList.Any(c => c.ParentCategoryId == dbCategory.Id))
                {
                    OrderCategories(dbCategoryList, categoryEntities, dbCategory.Id);
                }
            }
            return categoryEntities;
        }
    }
}

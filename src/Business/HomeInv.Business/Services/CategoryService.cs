using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business.Services
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

            if (string.IsNullOrWhiteSpace(request.CategoryEntity.Name))
            {
                response.AddError("Adı bomboş olan bir kategori olamamalı diye düşünüyorum");
            }
            else if (request.HomeId == 0)
            {
                response.AddError(Language.Resources.Category_HomeIsMandatory);
            }
            else
            {
                //Prevent creating categories under the same parent with the same name
                var existingCategory = context.Categories.Where(category => category.HomeId == request.HomeId && category.Name == request.CategoryEntity.Name && category.ParentCategoryId == request.CategoryEntity.ParentCategoryId);
                if (existingCategory != null && existingCategory.Count() > 0)
                {
                    response.AddError(Language.Resources.Category_SameNameExists);
                }

                //Prevent creating level 3 children
                if (request.CategoryEntity.ParentCategoryId != null && request.CategoryEntity.ParentCategoryId > 0)
                {
                    var parent = context.Categories.Find(request.CategoryEntity.ParentCategoryId);
                    if (parent != null && (parent.ParentCategoryId ?? 0) > 0)
                    {
                        var grandparent = context.Categories.Find(parent.ParentCategoryId);
                        if (grandparent != null && (grandparent.ParentCategoryId ?? 0) > 0)
                        {
                            response.AddError("Üçüncü seviyede kategori eklenemez.");
                        }
                    }
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
                foreach (var dbCategory in dbCategoryList.Where(cat => !cat.ParentCategoryId.HasValue || cat.ParentCategoryId == 0).ToList())
                {
                    var category = new CategoryEntity()
                    {
                        Id = dbCategory.Id,
                        Name = dbCategory.Name,
                        Description = dbCategory.Description,
                        HasParent = false
                    };
                    category.Children = GetChildrenOfCategory(dbCategoryList, category.Id);
                    category.HasChild = category.Children != null && category.Children.Count > 0;
                    categoryList.Add(category);
                }

                response.Categories = categoryList;
            }

            return response;
        }

        private List<CategoryEntity> GetChildrenOfCategory(List<Category> dbCategoryList, int parentCategoryId)
        {
            var childCategoryList = new List<CategoryEntity>();

            foreach (var dbCategory in dbCategoryList.Where(cat => cat.ParentCategoryId == parentCategoryId).ToList())
            {
                var childCategory = new CategoryEntity()
                {
                    Id = dbCategory.Id,
                    Name = dbCategory.Name,
                    Description = dbCategory.Description,
                    HasParent = true,
                    ParentCategoryId = dbCategory.ParentCategoryId
                };

                childCategory.Children = GetChildrenOfCategory(dbCategoryList, childCategory.Id);
                childCategory.HasChild = childCategory.Children != null && childCategory.Children.Count > 0;
                childCategoryList.Add(childCategory);
            }

            if (childCategoryList.Count > 0)
            {
                return childCategoryList;
            }
            else
            {
                return null;
            }
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

        public UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest request)
        {
            var response = new UpdateCategoryResponse();

            if (request.CategoryId <= 0)
            {
                response.AddError("Yanlis kategori!");
            }
            if (string.IsNullOrEmpty(request.Name))
            {
                response.AddError("İsim boş olamaz!");
            }
            if (context.Categories.Any(category => category.Id != request.CategoryId
                && category.HomeId == request.HomeId
                && category.Name == request.Name
                && category.ParentCategoryId == request.ParentCategoryId))
            {
                response.AddError(Language.Resources.Category_SameNameExists);
            }

            if (response.IsSuccessful)
            {
                var category = context.Categories.Find(request.CategoryId);
                category.Name = request.Name;
                category.Description = request.Description;
                category.ParentCategoryId = request.ParentCategoryId;

                context.SaveChanges();
            }

            return response;
        }
    }
}

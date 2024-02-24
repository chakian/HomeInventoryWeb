using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeInv.Common.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace HomeInv.Business.Services
{
    public class CategoryService : ServiceBase<Category, CategoryEntity>, ICategoryService<Category>
    {
        public CategoryService(HomeInventoryDbContext context) : base(context)
        {
        }

        public override CategoryEntity ConvertDboToEntity(Category dbo)
        {
            throw new NotImplementedException();
        }

        public async Task<CreateCategoryResponse> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct)
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
                var existingCategory = await context.Categories
                    .Where(category => 
                        category.HomeId == request.HomeId && 
                        category.Name == request.CategoryEntity.Name && 
                        category.ParentCategoryId == request.CategoryEntity.ParentCategoryId)
                    .ToListAsync(ct);
                if (existingCategory.Any())
                {
                    response.AddError(Language.Resources.Category_SameNameExists);
                }

                //Prevent creating level 3 children
                if (request.CategoryEntity.ParentCategoryId != null && request.CategoryEntity.ParentCategoryId > 0)
                {
                    var parent = await context.Categories.FindAsync(request.CategoryEntity.ParentCategoryId, ct);
                    if (parent != null && (parent.ParentCategoryId ?? 0) > 0)
                    {
                        var grandparent = await context.Categories.FindAsync(parent.ParentCategoryId, ct);
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
                await context.SaveChangesAsync(ct);
            }

            return response;
        }

        private static GetCategoriesOfHomeResponse Do_GetCategoriesOfHome_Validation(BaseHomeRelatedRequest request)
        {
            var response = new GetCategoriesOfHomeResponse();

            if (request.HomeId == 0)
            {
                response.AddError(Language.Resources.HomeSelectionIsMandatory);
            }

            return response;
        }

        public async Task<GetCategoriesOfHomeResponse> GetCategoriesOfHome_HierarchicalAsync(GetCategoriesOfHomeRequest request, CancellationToken ct)
        {
            var response = Do_GetCategoriesOfHome_Validation(request);

            if (response.IsSuccessful)
            {
                var dbCategoryList = await context.Categories
                    .Where(category => category.HomeId == request.HomeId && category.IsActive)
                    .OrderBy(cat => cat.ParentCategoryId)
                    .ThenBy(cat => cat.Id)
                    .ToListAsync(ct);
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

        public async Task<GetCategoriesOfHomeResponse> GetCategoriesOfHome_OrderedAsync(GetCategoriesOfHomeRequest request, CancellationToken ct)
        {
            var response = Do_GetCategoriesOfHome_Validation(request);

            if (response.IsSuccessful)
            {
                var dbCategoryList = await context.Categories
                    .Where(category => category.HomeId == request.HomeId && category.IsActive)
                    .OrderBy(cat => cat.ParentCategoryId)
                    .ThenBy(cat => cat.Id)
                    .ToListAsync(ct);
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

        public async Task<UpdateCategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken ct)
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
            if (await context.Categories.AnyAsync(category => category.Id != request.CategoryId
                && category.HomeId == request.HomeId
                && category.Name == request.Name
                && category.ParentCategoryId == request.ParentCategoryId, ct))
            {
                response.AddError(Language.Resources.Category_SameNameExists);
            }
            if (request.ParentCategoryId != null && request.ParentCategoryId > 0)
            {
                var parent = await context.Categories.FindAsync(request.ParentCategoryId, ct);
                if (parent != null && (parent.ParentCategoryId ?? 0) > 0)
                {
                    var grandparent = context.Categories.Find(parent.ParentCategoryId);
                    if (grandparent != null && (grandparent.ParentCategoryId ?? 0) > 0)
                    {
                        response.AddError("Üçüncü seviyede kategori olamıyor maalesef.");
                    }
                }
            }
            var category = await context.Categories.FindAsync(request.CategoryId, ct);
            if (category == null)
            {
                response.AddError("Buraya normalde gelememiş olmanız gerekirdi. Lütfen gider misiniz?");
            }
            else if(category.ParentCategoryId != request.ParentCategoryId && context.Categories.Any(c => c.ParentCategoryId == category.Id))
            {
                response.AddError("Bu kategorinin mevcutta alt kategorileri bulunmakta. Önce alt kategorileri taşımalısınız.");
            }
            else
            {
                category.Name = request.Name;
                category.Description = request.Description;
                category.ParentCategoryId = request.ParentCategoryId;

                await context.SaveChangesAsync(ct);
            }

            return response;
        }
    }
}

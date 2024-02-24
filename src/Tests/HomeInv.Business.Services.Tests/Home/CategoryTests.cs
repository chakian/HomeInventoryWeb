using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Persistence.Dbo;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace HomeInv.Business.Services.Tests
{
    public class CategoryTests : HomeRelatedTestBase, IDisposable
    {
        public void Dispose()
        {
        }

        protected override void SeedData()
        {
        }

        [Fact]
        public void Category_Create_Ok()
        {
            // arrange
            var context = GetContext();
            string categoryName = "Test category", description = "Test";

            int homeId = CreateDefaultHome(context);

            // act
            ICategoryService categoryService = new CategoryService(context);
            var request = new CreateCategoryRequest()
            {
                CategoryEntity = new CategoryEntity()
                {
                    Name = categoryName,
                    Description = description,
                    ParentCategoryId = null
                },
                HomeId = homeId,
                RequestUserId = userIds[0]
            };
            var actual = categoryService.CreateCategoryAsync(request, CancellationToken.None).Result;
            var expected = context.Categories.First();

            // assert
            Assert.NotNull(expected);
            Assert.True(actual.IsSuccessful);
        }

        [Fact]
        public void Category_Create_Fail_Exists()
        {
            // arrange
            var context = GetContext();
            string categoryName = "Test category", description = "Test";

            int homeId = CreateDefaultHome(context);

            var existingCategory = new Category()
            {
                Name = categoryName,
                Description = description,
                HomeId = homeId,
                ParentCategoryId = null
            };
            context.Categories.Add(existingCategory);
            context.SaveChanges();

            // act
            ICategoryService categoryService = new CategoryService(context);
            var request = new CreateCategoryRequest()
            {
                CategoryEntity = new CategoryEntity()
                {
                    Name = categoryName,
                    Description = description,
                    ParentCategoryId = null
                },
                HomeId = homeId,
                RequestUserId = userIds[0]
            };
            var actual = categoryService.CreateCategoryAsync(request, CancellationToken.None).Result;

            // assert
            Assert.False(actual.IsSuccessful);
            Assert.Equal(Language.Resources.Category_SameNameExists, actual.Result.Messages[0].Text);
        }

        [Fact]
        public void Category_Create_Fail_HomeRequired()
        {
            // arrange
            var context = GetContext();
            string categoryName = "Test category", description = "Test";

            // act
            ICategoryService categoryService = new CategoryService(context);
            var request = new CreateCategoryRequest()
            {
                CategoryEntity = new CategoryEntity()
                {
                    Name = categoryName,
                    Description = description,
                    ParentCategoryId = null
                },
                RequestUserId = userIds[0]
            };
            var actual = categoryService.CreateCategoryAsync(request, CancellationToken.None).Result;

            // assert
            Assert.False(actual.IsSuccessful);
            Assert.Equal(Language.Resources.Category_HomeIsMandatory, actual.Result.Messages[0].Text);
        }

        [Fact]
        public void Category_Create_Fail_MaxThreeLevelsAllowed()
        {
            // arrange
            var context = GetContext();
            string categoryTopLevel = "Test category", categorySecondLevel = "Second Level", categoryThirdLevel = "Third Level";
            int homeId = CreateDefaultHome(context);

            var existingCategory = new Category()
            {
                Name = categoryTopLevel,
                HomeId = homeId,
                ParentCategoryId = null
            };
            context.Categories.Add(existingCategory);
            context.SaveChanges();
            int topLevelId = existingCategory.Id;
            
            existingCategory = new Category()
            {
                Name = categorySecondLevel,
                HomeId = homeId,
                ParentCategoryId = topLevelId
            };
            context.Categories.Add(existingCategory);
            context.SaveChanges();
            int secondLevelId = existingCategory.Id;

            existingCategory = new Category()
            {
                Name = categoryThirdLevel,
                HomeId = homeId,
                ParentCategoryId = secondLevelId
            };
            context.Categories.Add(existingCategory);
            context.SaveChanges();
            int thirdLevelId = existingCategory.Id;

            // act
            ICategoryService categoryService = new CategoryService(context);
            var request = new CreateCategoryRequest()
            {
                CategoryEntity = new CategoryEntity()
                {
                    Name = "new category",
                    ParentCategoryId = thirdLevelId
                },
                HomeId = homeId,
                RequestUserId = userIds[0]
            };
            var actual = categoryService.CreateCategoryAsync(request, CancellationToken.None).Result;

            // assert
            Assert.False(actual.IsSuccessful);
            Assert.Equal(Language.Resources.Category_MaxThreeLevelsAllowed, actual.Result.Messages[0].Text);
        }

        [Fact]
        public void Category_GetCategoriesOfHome_Ok()
        {
            // arrange
            var context = GetContext();
            int homeId = CreateDefaultHome(context);

            string categoryName = "Test category", description = "Test";
            for (int i = 0; i < 10; i++)
            {
                var newCategory = new Category()
                {
                    Name = categoryName + i.ToString(),
                    Description = description,
                    HomeId = homeId,
                    ParentCategoryId = null,
                    IsActive = true
                };
                context.Categories.Add(newCategory);
            }
            context.SaveChanges();

            // act
            ICategoryService categoryService = new CategoryService(context);
            var request = new GetCategoriesOfHomeRequest()
            {
                HomeId = homeId,
                RequestUserId = userIds[0]
            };
            var actual = categoryService.GetCategoriesOfHome_OrderedAsync(request, CancellationToken.None).Result;

            // assert
            Assert.NotNull(actual);
            Assert.Equal(10, actual.Categories.Count);
        }

        [Fact]
        public void Category_GetCategoriesOfHome_Ok_OnlyActives()
        {
            // arrange
            var context = GetContext();
            int homeId = CreateDefaultHome(context);

            string categoryName = "Test category", description = "Test";
            for (int i = 0; i < 10; i++)
            {
                var newCategory = new Category()
                {
                    Name = categoryName + i.ToString(),
                    Description = description,
                    HomeId = homeId,
                    ParentCategoryId = null,
                    IsActive = i % 2 == 0
                };
                context.Categories.Add(newCategory);
            }
            context.SaveChanges();

            // act
            ICategoryService categoryService = new CategoryService(context);
            var request = new GetCategoriesOfHomeRequest()
            {
                HomeId = homeId,
                RequestUserId = userIds[0]
            };
            var actual = categoryService.GetCategoriesOfHome_OrderedAsync(request, CancellationToken.None).Result;

            // assert
            Assert.NotNull(actual);
            Assert.Equal(5, actual.Categories.Count);
        }
    }
}

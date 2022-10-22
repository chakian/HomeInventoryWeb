using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Category;
using HomeInv.Persistence.Dbo;
using System;
using System.Linq;
using Xunit;

namespace HomeInv.Business.Services.Tests
{
    public class CategoryTests : TestBase, IDisposable
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

            var home = new Home()
            {
                Name = "new home"
            };
            context.Homes.Add(home);
            context.SaveChanges();

            ICategoryService categoryService = new CategoryService(context);
            var request = new CreateCategoryRequest()
            {
                CategoryEntity=new CategoryEntity()
                {
                    Name = categoryName,
                    Description = description,
                    ParentCategoryId = null
                },
                HomeId = home.Id,
                RequestUserId = userIds[0]
            };

            // act
            var actual = categoryService.CreateCategory(request);
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

            var home = new Home()
            {
                Name = "new home"
            };
            context.Homes.Add(home);

            var existingCategory = new Category()
            {
                Name = categoryName,
                Description = description,
                HomeId = home.Id,
                ParentCategoryId = null
            };
            context.Categories.Add(existingCategory);
            context.SaveChanges();

            ICategoryService categoryService = new CategoryService(context);
            var request = new CreateCategoryRequest()
            {
                CategoryEntity = new CategoryEntity()
                {
                    Name = categoryName,
                    Description = description,
                    ParentCategoryId = null
                },
                HomeId = home.Id,
                RequestUserId = userIds[0]
            };

            // act
            var actual = categoryService.CreateCategory(request);

            // assert
            Assert.False(actual.IsSuccessful);
            Assert.Equal(Language.Resources.Category_SameNameExists, actual.Result.Messages[0].Text);
            //TODO: Assert error message as well
        }

        [Fact]
        public void Category_GetCategoriesOfHome_Ok()
        {
            // arrange
            var context = GetContext();
            var home = new Home()
            {
                Name = "new home"
            };
            context.Homes.Add(home);

            string categoryName = "Test category", description = "Test";
            for (int i = 0; i < 10; i++)
            {
                var newCategory = new Category()
                {
                    Name = categoryName + i.ToString(),
                    Description = description,
                    HomeId = home.Id,
                    ParentCategoryId = null,
                    IsActive = true
                };
                context.Categories.Add(newCategory);
            }
            context.SaveChanges();

            ICategoryService categoryService = new CategoryService(context);
            var request = new GetCategoriesOfHomeRequest()
            {
                HomeId = home.Id,
                RequestUserId = userIds[0]
            };

            // act
            var actual = categoryService.GetCategoriesOfHome_Ordered(request);

            // assert
            Assert.NotNull(actual);
            Assert.Equal(10, actual.Categories.Count);
        }

        [Fact]
        public void Category_GetCategoriesOfHome_Ok_OnlyActives()
        {
            // arrange
            var context = GetContext();
            var home = new Home()
            {
                Name = "new home"
            };
            context.Homes.Add(home);

            string categoryName = "Test category", description = "Test";
            for (int i = 0; i < 10; i++)
            {
                var newCategory = new Category()
                {
                    Name = categoryName + i.ToString(),
                    Description = description,
                    HomeId = home.Id,
                    ParentCategoryId = null,
                    IsActive = i % 2 == 0
                };
                context.Categories.Add(newCategory);
            }
            context.SaveChanges();

            ICategoryService categoryService = new CategoryService(context);
            var request = new GetCategoriesOfHomeRequest()
            {
                HomeId = home.Id,
                RequestUserId = userIds[0]
            };

            // act
            var actual = categoryService.GetCategoriesOfHome_Ordered(request);

            // assert
            Assert.NotNull(actual);
            Assert.Equal(5, actual.Categories.Count);
        }
    }
}

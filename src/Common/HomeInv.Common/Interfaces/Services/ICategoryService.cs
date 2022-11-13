using HomeInv.Common.ServiceContracts.Category;

namespace HomeInv.Common.Interfaces.Services
{
    public interface ICategoryService<D> : ICategoryService, IServiceBase<D, Entities.CategoryEntity>
        where D : class
    {
    }

    public interface ICategoryService : IServiceBase
    {
        CreateCategoryResponse CreateCategory(CreateCategoryRequest request);
        GetCategoriesOfHomeResponse GetCategoriesOfHome_Hierarchial(GetCategoriesOfHomeRequest request);
        GetCategoriesOfHomeResponse GetCategoriesOfHome_Ordered(GetCategoriesOfHomeRequest request);
        UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest request);
    }
}

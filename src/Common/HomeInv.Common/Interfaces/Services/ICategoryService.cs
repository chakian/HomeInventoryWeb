using System.Threading;
using System.Threading.Tasks;
using HomeInv.Common.ServiceContracts.Category;

namespace HomeInv.Common.Interfaces.Services
{
    public interface ICategoryService<D> : ICategoryService, IServiceBase<D, Entities.CategoryEntity>
        where D : class
    {
    }

    public interface ICategoryService : IServiceBase
    {
        Task<CreateCategoryResponse> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct);
        Task<GetCategoriesOfHomeResponse> GetCategoriesOfHome_HierarchicalAsync(GetCategoriesOfHomeRequest request, CancellationToken ct);
        Task<GetCategoriesOfHomeResponse> GetCategoriesOfHome_OrderedAsync(GetCategoriesOfHomeRequest request, CancellationToken ct);
        Task<UpdateCategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken ct);
    }
}

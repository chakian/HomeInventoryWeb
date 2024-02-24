using HomeInv.Common.Entities;

namespace HomeInv.WebApi.Contracts.Category;

public sealed class GetCategoriesResponse : BaseResponse
{
    public List<CategoryEntity> Categories { get; set; } = new();
}

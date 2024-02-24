using HomeInv.Common.Entities;

namespace HomeInv.WebApi.Contracts.Category;

public sealed class GetAllResponse : BaseResponse
{
    public List<CategoryEntity> Categories { get; set; }
}

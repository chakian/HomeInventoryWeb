using HomeInv.Common.Entities;

namespace HomeInv.WebApi.Contracts.Home;

public sealed class GetHomesResponse : BaseResponse
{
    public List<HomeEntity> Homes { get; set; } = new();
}

using HomeInv.Common.Entities;

namespace HomeInv.WebApi.Contracts.Home;

public sealed class GetAllResponse
{
    public List<HomeEntity> Homes { get; set; }
}

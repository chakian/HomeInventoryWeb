using HomeInv.Common.Entities;

namespace HomeInv.WebApi.Contracts.SizeUnit;

public class GetSizeUnitsResponse : BaseResponse
{
    public List<SizeUnitEntity> SizeUnits { get; set; } = new();
}

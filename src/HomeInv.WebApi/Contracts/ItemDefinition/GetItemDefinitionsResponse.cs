using HomeInv.Common.Entities;

namespace HomeInv.WebApi.Contracts.ItemDefinition;

public sealed class GetItemDefinitionsResponse : BaseResponse
{
    public List<ItemDefinitionEntity> ItemDefinitions { get; set; } = new();
}

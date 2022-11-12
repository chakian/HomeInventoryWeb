using HomeInv.Common.ServiceContracts.ItemDefinition;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IItemDefinitionService<D> : IItemDefinitionService, IServiceBase<D, Entities.ItemDefinitionEntity>
        where D : class
    {
    }

    public interface IItemDefinitionService : IServiceBase
    {
        GetAllItemDefinitionsInHomeResponse GetAllItemDefinitionsInHome(GetAllItemDefinitionsInHomeRequest request, bool includeInactive = false);

        GetFilteredItemDefinitionsInHomeResponse GetFilteredItemDefinitionsInHome(GetFilteredItemDefinitionsInHomeRequest request);

        GetItemDefinitionResponse GetItemDefinition(GetItemDefinitionRequest request);

        CreateItemDefinitionResponse CreateItemDefinition(CreateItemDefinitionRequest request);
    }
}

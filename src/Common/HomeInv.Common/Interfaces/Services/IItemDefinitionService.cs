using HomeInv.Common.ServiceContracts.ItemDefinition;
using System.Threading.Tasks;

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

        UpdateItemDefinitionResponse UpdateItemDefinition(UpdateItemDefinitionRequest request);

        Task<DeleteItemDefinitionResponse> DeleteItemDefinition(DeleteItemDefinitionRequest request);
    }
}

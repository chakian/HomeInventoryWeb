using System.Threading;
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
        Task<GetAllItemDefinitionsInHomeResponse> GetAllItemDefinitionsInHomeAsync(GetAllItemDefinitionsInHomeRequest request, CancellationToken ct);

        Task<GetFilteredItemDefinitionsInHomeResponse> GetFilteredItemDefinitionsInHomeAsync(GetFilteredItemDefinitionsInHomeRequest request, CancellationToken ct);

        Task<CreateItemDefinitionResponse> CreateItemDefinitionAsync(CreateItemDefinitionRequest request, CancellationToken ct);

        Task<UpdateItemDefinitionResponse> UpdateItemDefinitionAsync(UpdateItemDefinitionRequest request, CancellationToken ct);

        Task<DeleteItemDefinitionResponse> DeleteItemDefinitionAsync(DeleteItemDefinitionRequest request, CancellationToken ct);
    }
}

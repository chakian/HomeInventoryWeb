using HomeInv.Common.Entities;
using System.Linq;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IItemService : IServiceBase
    {
        IQueryable<Item> GetAllItems(bool includeInactive);

        Item GetItem(string id);

        string CreateItem(Item newItem);

        void EditItem(string id, Item editedItem);

        void DeleteItem(string id);
    }
}

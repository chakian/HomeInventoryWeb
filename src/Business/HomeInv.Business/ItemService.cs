using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.Settings;
using HomeInv.Persistence.Dbo;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace HomeInv.Business
{
    public class ItemService : ServiceBase, IItemService
    {
        public ItemService(IOptions<CosmosSettings> _cosmosSettings)// : base(_cosmosSettings)
        {
        }

        public override string GetCollectionName() => "Items";

        //public string CreateItem(Item NewItem)
        //{
        //    ItemDbo itemDbo = new ItemDbo();
        //    string NewID = Guid.NewGuid().ToString();
        //    itemDbo.Id = NewID;

        //    CreateDocument<ItemDbo>(itemDbo).Wait();
        //    return NewID;
        //}

        public void DeleteItem(string id) => throw new NotImplementedException();
        public void EditItem(string id, Common.Entities.Item editedItem) => throw new NotImplementedException();
        public IQueryable<Common.Entities.Item> GetAllItems(bool includeInactive) => throw new NotImplementedException();
        public Common.Entities.Item GetItem(string id) => throw new NotImplementedException();
        public string CreateItem(Common.Entities.Item newItem) => throw new NotImplementedException();
    }
}

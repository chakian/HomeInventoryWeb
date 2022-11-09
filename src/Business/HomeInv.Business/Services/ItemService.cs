//using HomeInv.Common.Entities;
//using HomeInv.Common.Interfaces.Services;
//using HomeInv.Common.ServiceContracts.Item;
//using HomeInv.Language;
//using HomeInv.Persistence;
//using HomeInv.Persistence.Dbo;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace HomeInv.Business.Services
//{
//    public class ItemService : AuditableServiceBase<Item, ItemEntity>, IItemService<Item>
//    {
//        public ItemService(HomeInventoryDbContext _context) : base(_context)
//        {
//        }

//        public override ItemEntity ConvertDboToEntity(Item dbo)
//        {
//            ItemEntity entity = ConvertBaseDboToEntityBase(dbo);
//            entity.Name = dbo.Name;
//            entity.Description = dbo.Description;
//            return entity;
//        }

//        public CreateItemResponse CreateItem(CreateItemRequest request)
//        {
//            CreateItemResponse response = new CreateItemResponse();

//            throw new NotImplementedException();
//            //if (string.IsNullOrEmpty(request.ItemEntity.Name))
//            //{
//            //    response.AddError("İsim boş olamaz!");
//            //}
//            //if (context.Items.Any(q => q.Name == request.ItemEntity.Name))
//            //{
//            //    response.AddError(Resources.ItemNameExists);
//            //}

//            //if (response.IsSuccessful)
//            //{
//            //    Item item = CreateNewAuditableObject(request);
//            //    item.Name = request.ItemEntity.Name;
//            //    item.Description = request.ItemEntity.Description;

//            //    context.Items.Add(item);
//            //    context.SaveChanges();

//            //    response.ItemEntity = ConvertDboToEntity(item);
//            //}

//            return response;
//        }

//        public GetAllItemsInHomeResponse GetAllItemsInHome(GetAllItemsInHomeRequest request, bool includeInactive = false)
//        {
//            var response = new GetAllItemsInHomeResponse();

//            throw new NotImplementedException();
//            //var dbItemList = context.Items.Where(item => item.IsActive && item.HomeId == request.HomeId).ToList();
//            //List<ItemEntity> itemList = new List<ItemEntity>();
//            //foreach (var item in dbItemList)
//            //{
//            //    itemList.Add(ConvertDboToEntity(item));
//            //}

//            //response.Items = itemList;

//            return response;
//        }

//        public GetItemResponse GetItem(GetItemRequest request)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

//using HomeInv.Business.Base;
//using HomeInv.Common.Entities;
//using HomeInv.Common.Interfaces.Services;
//using HomeInv.Common.ServiceContracts.Item;
//using HomeInv.Language;
//using HomeInv.Persistence;
//using HomeInv.Persistence.Dbo;
//using System;
//using System.Linq;

//namespace HomeInv.Business
//{
//    public class ItemService : ServiceBase<Item, ItemEntity>, IItemService<Item>
//    {
//        public ItemService(HomeInventoryDbContext _context) : base(_context)
//        {
//        }

//        public override ItemEntity ConvertDboToEntity(Item dbo)
//        {
//            ItemEntity entity = ConvertBaseDboToEntityBase<Item, ItemEntity>(dbo);
//            entity.Name = dbo.Name;
//            entity.Description = dbo.Description;
//            return entity;
//        }

//        public CreateItemResponse CreateItem(CreateItemRequest request)
//        {
//            CreateItemResponse response = new CreateItemResponse();

//            if (string.IsNullOrEmpty(request.ItemEntity.Name))
//            {
//                response.AddError("İsim boş olamaz!");
//                return response;
//            }
//            else if (context.Items.Any(q => q.Name == request.ItemEntity.Name))
//            {
//                response.AddError(Resources.ItemNameExists);
//                return response;
//            }

//            Item item = CreateNewAuditableObject<Item>(request);
//            item.Name = request.ItemEntity.Name;
//            item.Description = request.ItemEntity.Description;

//            context.Items.Add(item);
//            context.SaveChanges();

//            response.ItemEntity = ConvertDboToEntity(item);

//            return response;
//        }
//    }
//}

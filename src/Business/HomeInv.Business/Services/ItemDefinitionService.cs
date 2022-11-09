using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business.Services
{
    public class ItemDefinitionService : AuditableServiceBase<ItemDefinition, ItemDefinitionEntity>, IItemDefinitionService<ItemDefinition>
    {
        public ItemDefinitionService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override ItemDefinitionEntity ConvertDboToEntity(ItemDefinition dbo)
        {
            ItemDefinitionEntity entity = ConvertBaseDboToEntityBase(dbo);
            entity.Name = dbo.Name;
            entity.Description = dbo.Description;
            return entity;
        }

        public CreateItemDefinitionResponse CreateItemDefinition(CreateItemDefinitionRequest request)
        {
            CreateItemDefinitionResponse response = new CreateItemDefinitionResponse();

            if (string.IsNullOrEmpty(request.ItemEntity.Name))
            {
                response.AddError("İsim boş olamaz!");
            }
            if (context.ItemDefinitions.Any(q => q.Name == request.ItemEntity.Name))
            {
                response.AddError(Resources.ItemNameExists);
            }

            if (response.IsSuccessful)
            {
                ItemDefinition item = CreateNewAuditableObject(request);
                item.Name = request.ItemEntity.Name;
                item.Description = request.ItemEntity.Description;

                context.ItemDefinitions.Add(item);
                context.SaveChanges();

                response.ItemEntity = ConvertDboToEntity(item);
            }

            return response;
        }

        public GetAllItemDefinitionsInHomeResponse GetAllItemDefinitionsInHome(GetAllItemDefinitionsInHomeRequest request, bool includeInactive = false)
        {
            var response = new GetAllItemDefinitionsInHomeResponse();

            var dbItemList = context.ItemDefinitions.Where(item => item.IsActive && item.Category.HomeId == request.HomeId).ToList();
            List<ItemDefinitionEntity> itemList = new List<ItemDefinitionEntity>();
            foreach (var item in dbItemList)
            {
                itemList.Add(ConvertDboToEntity(item));
            }

            response.Items = itemList;

            return response;
        }

        public GetItemDefinitionResponse GetItemDefinition(GetItemDefinitionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

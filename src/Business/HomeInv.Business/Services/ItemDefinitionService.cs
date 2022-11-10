using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
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
            var queryableCategories = context.Categories.AsQueryable();
            ItemDefinitionEntity entity = ConvertBaseDboToEntityBase(dbo);
            entity.Name = dbo.Name;
            entity.Description = dbo.Description;
            if(dbo.Category != null)
            {
                entity.CategoryId= dbo.Category.Id;
                entity.CategoryName= dbo.Category.Name;

                if (dbo.Category.ParentCategoryId!= null)
                {
                    var parentsNames = "";

                    var parent = queryableCategories.Single(c => c.Id == dbo.Category.ParentCategoryId);
                    parentsNames = parent.Name;
                    if(parent.ParentCategoryId != null)
                    {
                        var grandparent = queryableCategories.Single(c => c.Id == parent.ParentCategoryId);
                        parentsNames = grandparent.Name + " - " + parentsNames;
                    }

                    entity.CategoryFullName = parentsNames;
                }
            }
            return entity;
        }

        public CreateItemDefinitionResponse CreateItemDefinition(CreateItemDefinitionRequest request)
        {
            CreateItemDefinitionResponse response = new CreateItemDefinitionResponse();

            if (request.ItemEntity.CategoryId <= 0)
            {
                response.AddError("Kategori seçimi olmadan item tanımı yapılamaz!");
            }
            if (string.IsNullOrEmpty(request.ItemEntity.Name))
            {
                response.AddError("İsim boş olamaz!");
            }
            if (context.ItemDefinitions.Any(q => q.Category.HomeId == request.HomeId && q.Name == request.ItemEntity.Name))
            {
                response.AddError(Resources.ItemNameExists);
            }

            if (response.IsSuccessful)
            {
                ItemDefinition item = CreateNewAuditableObject(request);
                item.Name = request.ItemEntity.Name;
                item.Description = request.ItemEntity.Description;
                item.CategoryId = request.ItemEntity.CategoryId;
                item.IsExpirable = request.ItemEntity.IsExpirable;

                context.ItemDefinitions.Add(item);
                context.SaveChanges();
            }

            return response;
        }

        public GetAllItemDefinitionsInHomeResponse GetAllItemDefinitionsInHome(GetAllItemDefinitionsInHomeRequest request, bool includeInactive = false)
        {
            var response = new GetAllItemDefinitionsInHomeResponse();

            var dbItemList = context.ItemDefinitions.Include(itemDef => itemDef.Category)
                .Where(item => (includeInactive ? true : item.IsActive) && item.Category.HomeId == request.HomeId)
                .ToList();
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

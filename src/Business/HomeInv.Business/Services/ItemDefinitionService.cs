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
            entity.ImageName = dbo.ImageName;
            entity.SizeUnitId = dbo.SizeUnitId;
            entity.SizeUnitName = dbo.SizeUnit.Name;
            if (dbo.Category != null)
            {
                entity.CategoryId = dbo.Category.Id;
                entity.CategoryName = dbo.Category.Name;

                if (dbo.Category.ParentCategoryId != null)
                {
                    var parentsNames = "";

                    var parent = queryableCategories.Single(c => c.Id == dbo.Category.ParentCategoryId);
                    parentsNames = parent.Name;
                    if (parent.ParentCategoryId != null)
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
            var response = new CreateItemDefinitionResponse();

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
                item.ImageName = request.ItemEntity.ImageName;
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
                .Where(item => (includeInactive || item.IsActive) && item.Category.HomeId == request.HomeId)
                .ToList();
            var itemList = new List<ItemDefinitionEntity>();
            foreach (var item in dbItemList)
            {
                itemList.Add(ConvertDboToEntity(item));
            }

            response.Items = itemList;

            return response;
        }

        public GetFilteredItemDefinitionsInHomeResponse GetFilteredItemDefinitionsInHome(GetFilteredItemDefinitionsInHomeRequest request)
        {
            var response = new GetFilteredItemDefinitionsInHomeResponse() { Items = new List<ItemDefinitionEntity>() };

            var queryableList = context.ItemDefinitions
                .Include(item => item.Category)
                .Include(item => item.SizeUnit)
                .Where(item => item.Category.HomeId == request.HomeId)
                .AsQueryable();
            if (request.AreaId > 0)
            {
                //var joinedQuery = context.ItemStocks.Join(queryableList, stock => stock.ItemDefinitionId, item => item.Id, )
                queryableList.Join(context.ItemStocks, item => item.Id, stock => stock.ItemDefinitionId, (item, stock) => new
                {
                    ItemDef = item,
                    Stock = stock
                });
                //queryableList.Include(item => item.ItemStocks).Where(item => item.ItemStocks.Any(itemStock => itemStock.AreaId == request.AreaId));
            }
            if (request.CategoryId > 0)
            {
                queryableList.Where(item => item.Category.Id == request.CategoryId
                || item.Category.ParentCategoryId == request.CategoryId
                || (item.Category.ParentCategory == null || item.Category.ParentCategory.ParentCategoryId == request.CategoryId));
            }

            foreach (var item in queryableList.ToList())
            {
                response.Items.Add(ConvertDboToEntity(item));
            }

            return response;
        }

        public GetItemDefinitionResponse GetItemDefinition(GetItemDefinitionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

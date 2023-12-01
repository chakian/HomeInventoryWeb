using HomeInv.Business.Extensions;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            entity.SizeUnitName = string.IsNullOrEmpty(dbo.SizeUnit.Description) ? dbo.SizeUnit.Name : dbo.SizeUnit.Description;
            entity.IsExpirable = dbo.IsExpirable;
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
            entity.IsActive = dbo.IsActive;
            return entity;
        }

        public CreateItemDefinitionResponse CreateItemDefinition(CreateItemDefinitionRequest request)
        {
            var response = new CreateItemDefinitionResponse();

            if (request.ItemEntity.CategoryId <= 0)
            {
                response.AddError("Kategori seçimi olmadan item tanımı yapılamaz!");
            }
            if (request.ItemEntity.SizeUnitId <= 0)
            {
                response.AddError("Boyut seçimi olmadan item tanımı yapılamaz!");
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
                item.SizeUnitId = request.ItemEntity.SizeUnitId;

                context.ItemDefinitions.Add(item);
                context.SaveChanges();

                if (!string.IsNullOrEmpty(request.ImageFileExtension))
                {
                    if (allowedImageExtensions.Contains(request.ImageFileExtension))
                    {
                        item.ImageName = string.Concat("item_", item.Id.ToString(), request.ImageFileExtension);
                        context.SaveChanges();
                        response.ImageFileName = item.ImageName;
                    }
                    else
                    {
                        response.AddWarning("Seçilen dosya yüklenemedi. Dosya uzantısı bunlardan biri olmalı: '" + string.Join(',', allowedImageExtensions) + "'");
                    }
                }
            }

            return response;
        }

        string[] allowedImageExtensions = new string[] { ".png", ".jpg", ".jpeg", ".gif", ".tiff", ".bmp", ".svg" };

        public GetAllItemDefinitionsInHomeResponse GetAllItemDefinitionsInHome(GetAllItemDefinitionsInHomeRequest request, bool includeInactive = false)
        {
            var response = new GetAllItemDefinitionsInHomeResponse();

            var dbItemList = context.ItemDefinitions
                .Include(itemDef => itemDef.Category)
                .Include(itemDef => itemDef.SizeUnit)
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
                .Where(item => item.IsActive && item.Category.HomeId == request.HomeId)
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
            var response = new GetItemDefinitionResponse() { };

            var item = context.ItemDefinitions
                .Include(item => item.Category)
                .Include(item => item.SizeUnit)
                .SingleOrDefault(i => i.Id == request.ItemDefinitionId);
            if (item == null)
            {
                response.AddError("Urun tanimi bulunamadi");
            }
            else
            {
                response.ItemDefinition = ConvertDboToEntity(item);
            }

            return response;
        }

        public UpdateItemDefinitionResponse UpdateItemDefinition(UpdateItemDefinitionRequest request)
        {
            var response = new UpdateItemDefinitionResponse();

            if (request.CategoryId <= 0)
            {
                response.AddError("Kategori seçimi olmadan item tanımı yapılamaz!");
            }
            if (string.IsNullOrEmpty(request.Name))
            {
                response.AddError("İsim boş olamaz!");
            }
            if (context.ItemDefinitions.Any(q => q.Id != request.ItemDefinitionId
                && q.Category.HomeId == request.HomeId
                && q.Name == request.Name))
            {
                response.AddError(Resources.ItemNameExists);
            }

            if (response.IsSuccessful)
            {
                var item = context.ItemDefinitions.Find(request.ItemDefinitionId);
                item.Name = request.Name;
                item.Description = request.Description;
                item.CategoryId = request.CategoryId;
                item.IsExpirable = request.IsExpirable;

                if (!string.IsNullOrEmpty(request.NewImageFileExtension))
                {
                    if (allowedImageExtensions.Contains(request.NewImageFileExtension))
                    {
                        item.ImageName = string.Concat("item_", item.Id.ToString(), request.NewImageFileExtension);
                        response.ImageFileName = item.ImageName;
                    }
                    else
                    {
                        response.AddWarning("Seçilen dosya yüklenemedi. Dosya uzantısı bunlardan biri olmalı: '" + string.Join("; ", allowedImageExtensions) + "'");
                    }
                }

                context.SaveChanges();
            }

            return response;
        }

        public async Task<DeleteItemDefinitionResponse> DeleteItemDefinition(DeleteItemDefinitionRequest request)
        {
            var response = new DeleteItemDefinitionResponse();

            if (request.ItemDefinitionId <= 0)
            {
                response.AddError("Ürün tanımı olmadan item tanımı silişi yapılamaz!");
            }

            if (response.IsSuccessful)
            {
                var item = await context.ItemDefinitions.FindAsync(request.ItemDefinitionId);
                item.IsActive = false;
                item.SetUpdateAuditValues(request);
                await context.SaveChangesAsync();
            }

            return response;
        }
    }
}

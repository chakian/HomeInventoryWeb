using HomeInv.Business.Extensions;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.ItemDefinition;
using HomeInv.Common.Utils;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeInv.Business.Services
{
    public class ItemDefinitionService : AuditableServiceBase<ItemDefinition, ItemDefinitionEntity>, IItemDefinitionService<ItemDefinition>
    {
        public ItemDefinitionService(HomeInventoryDbContext context) : base(context)
        {
        }

        public override ItemDefinitionEntity ConvertDboToEntity(ItemDefinition dbo)
        {
            var queryableCategories = context.Categories.AsQueryable();
            ItemDefinitionEntity entity = ConvertBaseDboToEntityBase(dbo);
            entity.Name = dbo.Name;
            entity.Description = dbo.Description;
            entity.SizeUnitId = dbo.SizeUnitId;
            entity.SizeUnitName = string.IsNullOrEmpty(dbo.SizeUnit.Description) ? dbo.SizeUnit.Name : dbo.SizeUnit.Description;
            entity.IsExpirable = dbo.IsExpirable;
            if (dbo.Category != null)
            {
                entity.CategoryId = dbo.Category.Id;
                entity.CategoryName = dbo.Category.Name;

                if (dbo.Category.ParentCategoryId != null)
                {
                    var parent = queryableCategories.Single(c => c.Id == dbo.Category.ParentCategoryId);
                    var parentsNames = parent.Name;
                    if (parent.ParentCategoryId != null)
                    {
                        var grandparent = queryableCategories.Single(c => c.Id == parent.ParentCategoryId);
                        parentsNames = grandparent.Name + " - " + parentsNames;
                    }

                    entity.CategoryFullName = parentsNames;
                }

                entity.ImageUrl = new ImageUtility(dbo.Category.HomeId, dbo.Id).GetImageDisplayLink(Common.Constants.GlobalConstants.ImageSize.Minimum);
            }
            
            entity.IsActive = dbo.IsActive;
            return entity;
        }

        public async Task<CreateItemDefinitionResponse> CreateItemDefinitionAsync(CreateItemDefinitionRequest request, CancellationToken ct)
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
            if (await context.ItemDefinitions.AnyAsync(q => q.Category.HomeId == request.HomeId && q.Name == request.ItemEntity.Name, ct))
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
                item.SizeUnitId = request.ItemEntity.SizeUnitId;

                context.ItemDefinitions.Add(item);
                await context.SaveChangesAsync(ct);

                response.ItemDefinitionId = item.Id;
            }

            return response;
        }

        public async Task<GetAllItemDefinitionsInHomeResponse> GetAllItemDefinitionsInHomeAsync(GetAllItemDefinitionsInHomeRequest request, CancellationToken ct)
        {
            var response = new GetAllItemDefinitionsInHomeResponse();

            var dbItemList = await context.ItemDefinitions
                .Include(itemDef => itemDef.Category)
                .Include(itemDef => itemDef.SizeUnit)
                .Where(item => (request.IncludeInactive || item.IsActive) && item.Category.HomeId == request.HomeId)
                .ToListAsync(ct);
            var itemList = new List<ItemDefinitionEntity>();
            foreach (var item in dbItemList)
            {
                itemList.Add(ConvertDboToEntity(item));
            }

            response.Items = itemList;

            return response;
        }

        public async Task<GetFilteredItemDefinitionsInHomeResponse> GetFilteredItemDefinitionsInHomeAsync(GetFilteredItemDefinitionsInHomeRequest request, CancellationToken ct)
        {
            var response = new GetFilteredItemDefinitionsInHomeResponse() { Items = new List<ItemDefinitionEntity>() };

            var queryableList = context.ItemDefinitions
                .Include(item => item.Category)
                .Include(item => item.SizeUnit)
                .Where(item => item.IsActive && item.Category.HomeId == request.HomeId)
                .AsQueryable();
            if (request.AreaId > 0)
            {
                queryableList.Join(context.ItemStocks, item => item.Id, stock => stock.ItemDefinitionId, (item, stock) => new
                {
                    ItemDef = item,
                    Stock = stock
                });
            }
            if (request.CategoryId > 0)
            {
                queryableList.Where(item => item.Category.Id == request.CategoryId
                || item.Category.ParentCategoryId == request.CategoryId
                || (item.Category.ParentCategory == null || item.Category.ParentCategory.ParentCategoryId == request.CategoryId));
            }

            foreach (var item in await queryableList.ToListAsync(ct))
            {
                response.Items.Add(ConvertDboToEntity(item));
            }

            return response;
        }

        public async Task<UpdateItemDefinitionResponse> UpdateItemDefinitionAsync(UpdateItemDefinitionRequest request, CancellationToken ct)
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
                var item = await context.ItemDefinitions.FindAsync(new object[] { request.ItemDefinitionId }, ct);
                if (item != null)
                {
                    item.Name = request.Name;
                    item.Description = request.Description;
                    item.CategoryId = request.CategoryId;
                    item.IsExpirable = request.IsExpirable;

                    await context.SaveChangesAsync(ct);
                }
                else
                {
                    response.AddWarning("Ürünü bulamadık malesef");
                }
            }

            return response;
        }

        public async Task<DeleteItemDefinitionResponse> DeleteItemDefinitionAsync(DeleteItemDefinitionRequest request, CancellationToken ct)
        {
            var response = new DeleteItemDefinitionResponse();

            if (request.ItemDefinitionId <= 0)
            {
                response.AddError("Ürün tanımı olmadan item tanımı silişi yapılamaz!");
            }

            if (response.IsSuccessful)
            {
                var item = await context.ItemDefinitions.FindAsync(new object[] { request.ItemDefinitionId }, ct);
                if (item != null)
                {
                    item.IsActive = false;
                    item.SetUpdateAuditValues(request);
                    await context.SaveChangesAsync(ct);
                }
                else
                {
                    response.AddWarning("Ürünü bulamadık malesef");
                }
            }

            return response;
        }
    }
}

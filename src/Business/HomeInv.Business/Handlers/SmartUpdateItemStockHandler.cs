using HomeInv.Business.Extensions;
using HomeInv.Common.Constants;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Linq;

namespace HomeInv.Business.Handlers
{
    public class SmartUpdateItemStockHandler : HandlerBase<SmartUpdateItemStockRequest, SmartUpdateItemStockResponse>, ISmartUpdateItemStockHandler
    {
        public SmartUpdateItemStockHandler(HomeInventoryDbContext context) : base(context)
        {
        }

        private bool IsInsertAction(int actionType)
        {
            return (new int[] {
                (int)ItemStockActionTypeEnum.Purchased,
                (int)ItemStockActionTypeEnum.GiftedIn })
                .Contains(actionType);
        }
        private bool IsPurchaseAction(int actionType)
        {
            return (new int[] {
                (int)ItemStockActionTypeEnum.Purchased })
                .Contains(actionType);
        }
        private bool IsUpdateAction(int actionType)
        {
            return (new int[] {
                (int)ItemStockActionTypeEnum.Consumed,
                (int)ItemStockActionTypeEnum.Sold,
                (int)ItemStockActionTypeEnum.Dismissed,
                (int)ItemStockActionTypeEnum.Broken,
                (int)ItemStockActionTypeEnum.Lost,
                (int)ItemStockActionTypeEnum.GiftedOut})
                .Contains(actionType);
        }

        protected override SmartUpdateItemStockResponse ExecuteInternal(SmartUpdateItemStockRequest request, SmartUpdateItemStockResponse response)
        {
            ItemDefinition itemDefinition;
            //Fetch current record for item definition
            if (request.ItemDefinitionDetail.Id > 0)
            {
                itemDefinition = _context.ItemDefinitions.Find(request.ItemDefinitionDetail.Id);
            }
            else
            {
                itemDefinition = new ItemDefinition()
                {
                    Name = request.ItemDefinitionDetail.Name,
                    CategoryId = request.ItemDefinitionDetail.CategoryId,
                    Description = request.ItemDefinitionDetail.Description,
                    IsExpirable = request.ItemDefinitionDetail.IsExpirable,
                    ImageName = request.ItemDefinitionDetail.ImageName,
                    SizeUnitId = request.ItemDefinitionDetail.SizeUnitId,
                }.SetCreateAuditValues(request);
                _context.ItemDefinitions.Add(itemDefinition);
                _context.SaveChanges();
            }

            decimal originalAmountBeforeUpdate = 0;
            ItemStock itemStock;
            //Fetch current record for stock
            if (request.ItemStockDetail.Id > 0)
            {
                itemStock = _context.ItemStocks.Find(request.ItemStockDetail.Id);
                originalAmountBeforeUpdate = itemStock.RemainingAmount;
                _context.Entry(itemStock).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                itemStock = _context.ItemStocks.SingleOrDefault(
                    stock => stock.ItemDefinitionId == itemDefinition.Id
                    && stock.AreaId == request.ItemStockDetail.AreaId);
                if (itemStock == null)
                {
                    itemStock = new ItemStock()
                    {
                        AreaId = request.ItemStockDetail.AreaId,
                        ItemDefinitionId = itemDefinition.Id,
                    };
                    _context.Entry(itemStock).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                else
                {
                    originalAmountBeforeUpdate = itemStock.RemainingAmount;
                    _context.Entry(itemStock).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
            }

            itemStock.RemainingAmount = request.ItemStockDetail.RemainingAmount;
            itemStock.ExpirationDate = request.ItemStockDetail.ExpirationDate;
            itemStock.AreaId = request.ItemStockDetail.AreaId;

            if (_context.Entry(itemStock).State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                itemStock.SetCreateAuditValues(request);
            }
            else
            {
                itemStock.SetUpdateAuditValues(request);
            }
            _context.SaveChanges();

            decimal changedAmount = itemStock.RemainingAmount - originalAmountBeforeUpdate;
            int actionTypeId;
            if (changedAmount > 0)
            {
                actionTypeId = (int)ItemStockActionTypeEnum.Purchased;
            }
            else
            {
                actionTypeId = (int)ItemStockActionTypeEnum.Consumed;
            }

            //Insert action details into ItemStockAction table
            var action = new ItemStockAction()
            {
                ItemStockId = itemStock.Id,
                ItemStockActionTypeId = actionTypeId,
                Size = changedAmount,
                ActionDate = request.ActionDate,
                ActionTarget = request.ActionTarget,
                Price = request.Price,
            }.SetCreateAuditValues(request);
            _context.ItemStockActions.Add(action);
            _context.SaveChanges();

            //Insert unit price if this is a purchase action
            if (request.Price > 0 && changedAmount > 0)
            {
                var unitPrice = new ItemUnitPrice()
                {
                    ItemStockActionId = action.Id,
                    UnitPrice = request.Price / changedAmount,
                    ItemDefinitionId = itemDefinition.Id,
                    PriceDate = request.ActionDate,
                    IsActive = true
                };
                _context.ItemUnitPrices.Add(unitPrice);
                _context.SaveChanges();
            }

            return response;
        }

        protected override SmartUpdateItemStockResponse ValidateRequest(SmartUpdateItemStockRequest request, SmartUpdateItemStockResponse response)
        {
            if (request.ItemDefinitionDetail.Id == 0 && (
                string.IsNullOrEmpty(request.ItemDefinitionDetail.Name) ||
                request.ItemDefinitionDetail.SizeUnitId == 0 ||
                request.ItemDefinitionDetail.CategoryId == 0)
                )
            {
                response.AddError("Yeni ürün tanımı girişinde 'Ürün adı', 'Kategorisi' ve 'Boyut birimi' girilmesi zorunludur");
            }
            if (request.ItemStockDetail.Id == 0 && (
                request.ItemStockDetail.AreaId == 0)
                )
            {

                response.AddError("Mevcut stoğu olmayan bir ürün için giriş yapılırken 'Oda' seçimi zorunludur");
            }

            return response;
        }
    }
}

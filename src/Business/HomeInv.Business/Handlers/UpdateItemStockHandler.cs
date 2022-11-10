using HomeInv.Common.Constants;
using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.ServiceContracts;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Language;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeInv.Business.Handlers
{
    public class UpdateItemStockHandler : HandlerBase<UpdateItemStockRequest, UpdateItemStockResponse>, IUpdateItemStockHandler
    {
        public UpdateItemStockHandler(HomeInventoryDbContext context) : base(context)
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

        protected override UpdateItemStockResponse ExecuteInternal(UpdateItemStockRequest request, UpdateItemStockResponse response)
        {
            //Fetch current record for stock
            var currentStock = _context.ItemStocks.SingleOrDefault(
                stock => stock.ItemDefinitionId == request.ItemDefinitionId
                && stock.AreaId == request.AreaId);

            //Insert or Update related info into ItemStocks table
            if (IsInsertAction(request.ItemStockActionTypeId))
            {
                if (currentStock == null)
                {
                    currentStock = new ItemStock()
                    {
                        ItemDefinitionId = request.ItemDefinitionId,
                        AreaId = request.AreaId,
                        SizeUnitId = request.SizeUnitId,
                        ExpirationDate = request.ExpirationDate,
                        RemainingAmount = request.Size,
                        IsActive = true,
                        InsertUserId = request.RequestUserId,
                        InsertTime = DateTime.UtcNow
                    };

                    _context.Entry(currentStock).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                else
                {
                    currentStock.RemainingAmount += request.Size;
                    currentStock.UpdateUserId = request.RequestUserId;
                    currentStock.UpdateTime = DateTime.UtcNow;

                    _context.Entry(currentStock).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
            }
            else if (IsUpdateAction(request.ItemStockActionTypeId))
            {
                if (currentStock == null)
                {
                    response.AddError("Olmayan seyi guncelleyemezsiniz");
                }
                else
                {
                    currentStock.RemainingAmount -= request.Size;
                    currentStock.UpdateUserId = request.RequestUserId;
                    currentStock.UpdateTime = DateTime.UtcNow;

                    _context.Entry(currentStock).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
            }

            //Insert action details into ItemStockAction table
            var action = new ItemStockAction()
            {
                ItemStockId = currentStock.Id,
                ItemStockActionTypeId = request.ItemStockActionTypeId,
                Size= request.Size,
                ActionDate = request.ActionDate,
                ActionTarget= request.ActionTarget,
                Price= request.Price,
                IsActive=true,
                InsertUserId = request.RequestUserId,
                InsertTime = DateTime.UtcNow
            };
            _context.Entry(action).State = Microsoft.EntityFrameworkCore.EntityState.Added;

            //Insert unit price if this is a purchase action
            if (IsPurchaseAction(request.ItemStockActionTypeId))
            {
                var unitPrice = new ItemUnitPrice()
                {
                    ItemStockActionId = action.Id,
                    UnitPrice = request.Price / request.Size,
                    ItemDefinitionId = request.ItemDefinitionId,
                    PriceDate = request.ActionDate,
                    IsActive=true
                };
                _context.Entry(unitPrice).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }

            _context.SaveChanges();

            return response;
        }

        protected override UpdateItemStockResponse ValidateRequest(UpdateItemStockRequest request, UpdateItemStockResponse response)
        {
            if (request.ItemStockActionTypeId == (int)ItemStockActionTypeEnum.Purchased
                || request.ItemStockActionTypeId == (int)ItemStockActionTypeEnum.GiftedIn)
            {
                if (request.ItemDefinitionId <= 0)
                {
                    response.AddError(Resources.ItemStock_Error_ItemDefinitionIdEmpty);
                }
                if (request.AreaId <= 0)
                {
                    response.AddError(Resources.ItemStock_Error_AreaIdEmpty);
                }
                if (request.SizeUnitId <= 0)
                {
                    response.AddError(Resources.ItemStock_Error_SizeUnitIdEmpty);
                }
                if (request.Size <= 0)
                {
                    response.AddError(Resources.ItemStock_Error_SizeEmpty);
                }
            }
            if (request.ItemStockActionTypeId == (int)ItemStockActionTypeEnum.Consumed
                || request.ItemStockActionTypeId == (int)ItemStockActionTypeEnum.Sold
                || request.ItemStockActionTypeId == (int)ItemStockActionTypeEnum.Dismissed
                || request.ItemStockActionTypeId == (int)ItemStockActionTypeEnum.Broken
                || request.ItemStockActionTypeId == (int)ItemStockActionTypeEnum.Lost
                || request.ItemStockActionTypeId == (int)ItemStockActionTypeEnum.GiftedOut)
            {
                if (request.ItemStockId <= 0)
                {
                    response.AddError(Resources.ItemStock_Error_ItemStockIdEmptyOnUpdateAction);
                }
            }

            return response;
        }
    }
}

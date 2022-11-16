using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Linq;

namespace HomeInv.Business.Handlers
{
    public class UpdateRemainingStockAmountHandler : HandlerBase<UpdateRemainingStockAmountRequest, UpdateRemainingStockAmountResponse>, IUpdateRemainingStockAmountHandler
    {
        public UpdateRemainingStockAmountHandler(HomeInventoryDbContext context) : base(context)
        {
        }

        //private bool IsPurchaseAction(int actionType)
        //{
        //    return (new int[] {
        //        (int)ItemStockActionTypeEnum.Purchased })
        //        .Contains(actionType);
        //}

        protected override UpdateRemainingStockAmountResponse ExecuteInternal(UpdateRemainingStockAmountRequest request, UpdateRemainingStockAmountResponse response)
        {
            //Fetch current record for stock
            var currentStock = _context.ItemStocks.SingleOrDefault(
                stock => stock.Id == request.ItemStockId);
            
            decimal changeAmount = 0;

            if (currentStock == null)
            {
                response.AddError("Olmayan seyi guncelleyemezsiniz");
                return response;
            }
            else
            {
                changeAmount = request.RemainingAmount - currentStock.RemainingAmount;

                currentStock.RemainingAmount = request.RemainingAmount;
                currentStock.UpdateUserId = request.RequestUserId;
                currentStock.UpdateTime = DateTime.UtcNow;

                _context.Entry(currentStock).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            //Insert action details into ItemStockAction table
            var action = new ItemStockAction()
            {
                ItemStockId = currentStock.Id,
                ItemStockActionTypeId = request.ItemStockActionTypeId,
                Size = changeAmount,
                ActionDate = request.ActionDate,
                ActionTarget = request.ActionTarget,
                //Price = request.Price,
                IsActive = true,
                InsertUserId = request.RequestUserId,
                InsertTime = DateTime.UtcNow
            };
            _context.ItemStockActions.Add(action);
            _context.SaveChanges();

            //Insert unit price if this is a purchase action
            //if (IsPurchaseAction(request.ItemStockActionTypeId))
            //{
            //    var unitPrice = new ItemUnitPrice()
            //    {
            //        ItemStockActionId = action.Id,
            //        UnitPrice = request.Price / changeAmount,
            //        ItemDefinitionId = currentStock.ItemDefinitionId,
            //        PriceDate = request.ActionDate,
            //        IsActive = true
            //    };
            //    _context.ItemUnitPrices.Add(unitPrice);
            //}

            _context.SaveChanges();

            return response;
        }

        protected override UpdateRemainingStockAmountResponse ValidateRequest(UpdateRemainingStockAmountRequest request, UpdateRemainingStockAmountResponse response)
        {
            if (request.RemainingAmount < 0) { response.AddError("Kalan miktar sifirdan kucuk nasil olabiliyor biri bana aciklayabilir mi cok rica etsem?"); }
            return response;
        }
    }
}

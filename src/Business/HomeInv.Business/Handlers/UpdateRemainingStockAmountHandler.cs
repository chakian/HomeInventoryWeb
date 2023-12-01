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
    public class UpdateRemainingStockAmountHandler : HandlerBase<UpdateRemainingStockAmountRequest, UpdateRemainingStockAmountResponse>, IUpdateRemainingStockAmountHandler
    {
        public UpdateRemainingStockAmountHandler(HomeInventoryDbContext context) : base(context)
        {
        }

        private bool IsPurchaseAction(int actionType)
        {
            return (new int[] {
                (int)ItemStockActionTypeEnum.Purchased })
                .Contains(actionType);
        }
        private bool IsEntryAction(int actionType)
        {
            return (new int[] {
                (int)ItemStockActionTypeEnum.Purchased,
                (int)ItemStockActionTypeEnum.GiftedIn })
                .Contains(actionType);
        }
        private bool IsExitAction(int actionType)
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

        protected override UpdateRemainingStockAmountResponse ExecuteInternal(UpdateRemainingStockAmountRequest request, UpdateRemainingStockAmountResponse response)
        {
            //Fetch current record for stock
            var currentStock = _context.ItemStocks.SingleOrDefault(
                stock => stock.Id == request.ItemStockId);

            decimal _changeAmount = 0, _remainingAmount = 0;

            if (currentStock == null)
            {
                response.AddError("Olmayan seyi guncelleyemezsiniz");
                return response;
            }
            else if (currentStock.RemainingAmount - request.ConsumedAmount < 0)
            {
                response.AddError("Hani 'elde var sifir' diye bir deyis vardir ya... Sizde o da yok muymus.");
                return response;
            }
            else
            {
                if (request.RemainingAmount > 0)
                {
                    _changeAmount = request.RemainingAmount - currentStock.RemainingAmount;
                    _remainingAmount = request.RemainingAmount;
                }
                else if (request.ConsumedAmount > 0)
                {
                    _changeAmount = request.ConsumedAmount * -1;
                    _remainingAmount = currentStock.RemainingAmount - request.ConsumedAmount;
                }
                else if (request.EntryAmount > 0)
                {
                    _changeAmount = request.EntryAmount;
                    _remainingAmount = currentStock.RemainingAmount + request.EntryAmount;
                }

                currentStock.RemainingAmount = _remainingAmount;
                currentStock.SetUpdateAuditValues(request);

                _context.Entry(currentStock).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            //Insert action details into ItemStockAction table
            var action = new ItemStockAction()
            {
                ItemStockId = currentStock.Id,
                ItemStockActionTypeId = request.ItemStockActionTypeId,
                Size = _changeAmount,
                ActionDate = request.ActionDate,
                ActionTarget = request.ActionTarget,
                //Price = request.Price,
            }.SetCreateAuditValues(request);
            _context.ItemStockActions.Add(action);
            _context.SaveChanges();

            //Insert unit price if this is a purchase action
            if (IsPurchaseAction(request.ItemStockActionTypeId) && request.Price > 0 && _changeAmount > 0)
            {
                var unitPrice = new ItemUnitPrice()
                {
                    ItemStockActionId = action.Id,
                    UnitPrice = request.Price / _changeAmount,
                    ItemDefinitionId = currentStock.ItemDefinitionId,
                    PriceDate = request.ActionDate,
                    IsActive = true
                };
                _context.ItemUnitPrices.Add(unitPrice);
            }

            _context.SaveChanges();

            return response;
        }

        protected override UpdateRemainingStockAmountResponse ValidateRequest(UpdateRemainingStockAmountRequest request, UpdateRemainingStockAmountResponse response)
        {
            if (request.RemainingAmount < 0) { response.AddError("Kalan miktar sifirdan kucuk nasil olabiliyor biri bana aciklayabilir mi cok rica etsem?"); }
            if (request.ConsumedAmount < 0) { response.AddError("Negatif tuketim derken felsefe yapildigini dusunuyor sistem. Herhangi bir guncelleme yapilmadi. Blg."); }
            else if (request.ConsumedAmount > 0)
            {
                if (IsEntryAction(request.ItemStockActionTypeId)) response.AddError("Sadece stoktan azaltim yapan aksiyonlar secilebilir: Tuketildi\r\nAtildi\r\nKayboldu\r\nKirildi\r\nSatildi\r\nHediye Edildi");
            }
            if (request.EntryAmount < 0) { response.AddError("Negatif miktarda alim yapabildigini zanneden ilk insan oldugunuz icin bizden -5.000.000 dolar kazandiniz."); }
            else if (request.EntryAmount > 0)
            {
                if (IsExitAction(request.ItemStockActionTypeId)) response.AddError("Sadece stokta artis yapan aksiyonlar secilebilir: Satin Alindi\r\nHediye Olarak Geldi");
            }
            return response;
        }
    }
}

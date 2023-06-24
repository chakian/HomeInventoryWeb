using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.Models;
using HomeInv.Common.ServiceContracts.ItemStock;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeInv.Business.Services;

public class ItemStockService : AuditableServiceBase<ItemStock, ItemStockEntity>, IItemStockService<ItemStock>
{
    public IEmailSenderService emailSenderService { get; set; }
    public IConfiguration configuration { get; set; }
    public ItemStockService(HomeInventoryDbContext _context, 
        IEmailSenderService _emailSenderService,
        IConfiguration _configuration) : base(_context)
    {
        emailSenderService = _emailSenderService;
        configuration = _configuration;
    }

    public override ItemStockEntity ConvertDboToEntity(ItemStock dbo)
    {
        var entity = new ItemStockEntity()
        {
            Id = dbo.Id,
            ItemDefinitionId = dbo.ItemDefinitionId,
            Quantity = dbo.RemainingAmount,
            AreaId = dbo.AreaId,
            ExpirationDate = (dbo.ExpirationDate.HasValue ? dbo.ExpirationDate.Value : DateTime.MinValue)
        };
        if (dbo.Area != null) { entity.AreaName = dbo.Area.Name; }
        return entity;
    }

    public GetItemStocksByItemDefinitionIdsResponse GetItemStocksByItemDefinitionIds(GetItemStocksByItemDefinitionIdsRequest request)
    {
        var response = new GetItemStocksByItemDefinitionIdsResponse() { ItemStocks = new List<ItemStockEntity>() };

        var stocks = context.ItemStocks
            .Include(stock => stock.Area)
            .Where(stock => request.ItemDefinitionIdList.Contains(stock.ItemDefinitionId));

        foreach (var stock in stocks)
        {
            response.ItemStocks.Add(ConvertDboToEntity(stock));
        }

        return response;
    }

    public GetSingleItemStockResponse GetSingleItemStock(GetSingleItemStockRequest request)
    {
        var response = new GetSingleItemStockResponse() { Stock = new ItemStockEntity() };

        var stock = context.ItemStocks
            .SingleOrDefault(stock => stock.Id == request.ItemStockId);

        if (stock != null)
        {
            response.Stock = ConvertDboToEntity(stock);
        }

        return response;
    }

    public Dictionary<int, List<StockStatus>> CheckStocksPrepareShoppingListAndSendEmail(int? homeId)
    {
        Dictionary<int, List<StockStatus>> result = new();

        List<Home> activeHomes;
        if (homeId.HasValue)
        {
            activeHomes = context.Homes.Where(h => h.IsActive && h.Id == homeId.Value).ToList();
        }
        else
        {
            // Find active homes
            activeHomes = context.Homes.Where(h => h.IsActive).ToList();
            // Find homes that are set as default by users
            var defaultHomes = context.UserSettings.Where(us => us.IsActive).Select(us => us.DefaultHomeId).ToList();
            // Filter active homes by default usages
            activeHomes = activeHomes.Where(ah => defaultHomes.Contains(ah.Id)).ToList();
        }

        List<StockStatus> statusList;

        foreach (var activeHome in activeHomes)
        {
            // Clear status list
            statusList = new();
            // Get recipient list of the home
            List<string> recipientList = context.HomeUsers.Where(hu => hu.IsActive && hu.HomeId == activeHome.Id).Select(hu => hu.User.Email).ToList();

            // Find all item definitions in the home
            context.ItemDefinitions
                .Where(x => x.IsActive && x.Category.HomeId == activeHome.Id)
                .Include(x => x.SizeUnit)
                .ToList()
                .ForEach(x =>
            {
                statusList.Add(new StockStatus()
                {
                    ItemDefinitionId = x.Id,
                    ItemDefinitionName = x.Name,
                    SizeUnit = new SizeUnitEntity()
                    {
                        Id = x.SizeUnit.Id,
                        Name = x.SizeUnit.Name,
                        Description = x.SizeUnit.Description,
                        ConversionMultiplierToBase = x.SizeUnit.ConversionMultiplierToBase,
                        IsBaseUnit = x.SizeUnit.IsBaseUnit,
                    }
                });
            });

            // Set current stock amounts of all item definitions
            context.ItemStocks.Where(x => x.IsActive && statusList.Select(sl => sl.ItemDefinitionId).Contains(x.ItemDefinitionId)).ToList().ForEach(x =>
            {
                statusList.Single(sl => sl.ItemDefinitionId == x.ItemDefinitionId).CurrentStock = x.RemainingAmount;
                statusList.Single(sl => sl.ItemDefinitionId == x.ItemDefinitionId).ItemStockId = x.Id;
            });

            // Determine what needs to be bought
            CheckStockActionsAndDecideNeededAmount(statusList);

            result.Add(activeHome.Id, statusList.ToList());

            if(homeId.HasValue == false && bool.TryParse(configuration.GetSection("SendSmartStockStatusEmail").Value, out bool sendMail) && sendMail)
            {
                StringBuilder bodyBuilder = new();

                var neededList = statusList.Where(s => s.CurrentNeed == StockNeed.Needed).ToList();
                if (neededList != null)
                {
                    AppendTableWithDetails(neededList, bodyBuilder, "Alınması Gerekenler", neededList.Any(s => s.NeededAmount > 0));
                }
                var maybeList = statusList.Where(s => s.CurrentNeed == StockNeed.NotSure).ToList();
                if (maybeList != null)
                {
                    AppendTableWithDetails(maybeList, bodyBuilder, "Tam Emin Olunamayanlar", maybeList.Any(s => s.NeededAmount > 0));
                }
                var fineList = statusList.Where(s => s.CurrentNeed == StockNeed.Fine).ToList();
                if (fineList != null)
                {
                    AppendTableWithDetails(fineList, bodyBuilder, "Yeterli Stoğu Olanlar", fineList.Any(s => s.NeededAmount > 0));
                }

                MailRequest mailRequest = new()
                {
                    Subject = $"{DateTime.Today.ToString("dd MMMM yyyy")} tarihli stok raporu",
                    ToEmailList = recipientList,
                    Body = bodyBuilder.ToString()
                };
                emailSenderService.SendEmailAsync(mailRequest).GetAwaiter().GetResult();
            }
        }

        return result;
    }

    private static void AppendTableWithDetails(List<StockStatus> statusList, StringBuilder bodyBuilder, string header, bool includeNeededAmount)
    {
        bodyBuilder.AppendLine($"<h1>{header}</h1>");
        bodyBuilder.AppendLine("<table>");
        bodyBuilder.AppendLine("<thead>");
        bodyBuilder.AppendLine("<th>Ürün</th>");
        bodyBuilder.AppendLine("<th>Mevcut Stok</th>");
        if (includeNeededAmount) bodyBuilder.AppendLine("<th>Alınması gereken miktar</th>");
        bodyBuilder.AppendLine("<th>Not</th>");
        bodyBuilder.AppendLine("</thead>");
        foreach (var item in statusList)
        {
            bodyBuilder.AppendLine("<tr>");
            bodyBuilder.Append($"<td>{item.ItemDefinitionName}</td>");
            bodyBuilder.Append($"<td>{item.CurrentStock}</td>");
            if (includeNeededAmount) bodyBuilder.Append($"<td>{item.NeededAmount} {item.SizeUnit.Description ?? item.SizeUnit.Name}</td>");
            bodyBuilder.Append($"<td>{item.Note}</td>");
            bodyBuilder.Append("</tr>");
        }
        bodyBuilder.AppendLine("</table>");
        bodyBuilder.AppendLine("<br/><hr/><br/>");
    }

    private class StockChange
    {
        public DateTime ChangeDate { get; set; }
        public bool Increased { get; set; }
        public decimal Amount { get; set; }
    }

    private void CheckStockActionsAndDecideNeededAmount(List<StockStatus> statusList)
    {
        foreach (var stat in statusList)
        {
            var actions = context.ItemStockActions
                .Where(isa => isa.IsActive && isa.ItemStockId == stat.ItemStockId)
                .OrderByDescending(isa => isa.ActionDate)
                .ToList();
            var decreasingActions = actions.Where(a => a.Size < 0).ToList();
            var increasingActions = actions.Where(a => a.Size > 0).ToList();

            var changes = new List<StockChange>();

            foreach (var action in actions)
            {
                changes.Add(new StockChange
                {
                    ChangeDate = action.ActionDate,
                    Amount = Math.Abs(action.Size),
                    Increased = (action.Size > 0)
                });
            }

            var latestIncrease = changes.OrderByDescending(c => c.ChangeDate).FirstOrDefault(c => c.Increased && c.Amount > 0);
            if (latestIncrease != null)
            {
                var consumingAmountSinceLastPurchase = changes.Where(c => c.ChangeDate >= latestIncrease.ChangeDate && c.Increased == false).Sum(c=>c.Amount);
                if(consumingAmountSinceLastPurchase > 0)
                {
                    decimal currentlyHave = stat.CurrentStock.Value;
                    if (currentlyHave == 0)
                    {
                        var latestDecrease = changes.OrderByDescending(c => c.ChangeDate).FirstOrDefault(c => c.Increased == false && c.Amount > 0);
                        bool boughtInLast6Months = latestIncrease.ChangeDate.AddMonths(6) > DateTime.UtcNow;
                        bool consumedInLast6Months = latestDecrease.ChangeDate.AddMonths(6) > DateTime.UtcNow;

                        if (boughtInLast6Months)
                        {
                            if (consumedInLast6Months)
                            {
                                // Both bought and consumed in the last 6 months
                                stat.CurrentNeed = StockNeed.Needed;
                                stat.NeededAmount = latestIncrease.Amount;
                                stat.Note = "Son seferinde aldığın kadar alırsan yeter galiba.";
                            }
                            else
                            {
                                // Bought but not consumed in the last 6 months
                                stat.CurrentNeed = StockNeed.Fine;
                                stat.NeededAmount = 0;
                                stat.Note = "Son 6 ay içinde bu üründen alınmış ama hiç tüketilmemiş. Almak gerekmiyor gibi görünüyor.";
                            }
                        }
                        else
                        {
                            if (consumedInLast6Months)
                            {
                                // Not bought but consumed in the last 6 months
                                stat.CurrentNeed = StockNeed.Needed;
                                stat.NeededAmount = changes.Where(c => c.ChangeDate.AddMonths(6) > DateTime.UtcNow && c.Increased == false).Sum(c => c.Amount);
                                stat.Note = "Son 6 ayda tükettiğin kadar al bence";

                            }
                            else
                            {
                                // Neither bought nor consumed in the last 6 months
                                stat.CurrentNeed = StockNeed.NotSure;
                                stat.NeededAmount = 0;
                                stat.Note = "Son 6 ay içinde bu üründe herhangi bir değişiklik olmamış. Emin değilim yani. Sanki lazım değil gibi geldi bana.";
                            }
                        }
                    }
                    else
                    {
                        if (currentlyHave >= latestIncrease.Amount)
                        {
                            stat.CurrentNeed = StockNeed.Fine;
                            stat.NeededAmount = 0;
                            stat.Note = "Şimdilik yeterli gibi";
                        }
                        else
                        {
                            if (consumingAmountSinceLastPurchase >= latestIncrease.Amount)
                            {
                                stat.CurrentNeed = StockNeed.Needed;
                                stat.NeededAmount = latestIncrease.Amount;
                                stat.Note = "Son alımdan sonra bayağı tüketilmiş. Almak lazım sanki.";
                            }
                            else
                            {
                                stat.CurrentNeed = StockNeed.NotSure;
                                stat.NeededAmount = 0;
                                stat.Note = $"En son alımdan sonra {consumingAmountSinceLastPurchase} kadar tüketilmiş; ama alınması gerekiyor mu emin değilim.";
                            }
                        }
                    }
                }
                else
                {
                    stat.CurrentNeed = StockNeed.Fine;
                    stat.NeededAmount = 0;
                    stat.Note = $"En son {latestIncrease.ChangeDate.ToLocalTime().Date.ToString("dd MMMM yyyy")} tarihinde {latestIncrease.Amount} kadar eklenmiş ve sonra hiç tüketilmemiş. Alınması gerekiyor mu emin değilim.";
                }
            }
            else
            {
                var latestDecrease = changes.OrderByDescending(c => c.ChangeDate).FirstOrDefault(c => c.Amount < 0);
                if (latestDecrease == null)
                {
                    stat.CurrentNeed = StockNeed.Fine;
                    stat.NeededAmount = 0;
                    stat.Note = $"Bu ürün hiç tüketilmemiş. Almasak da olur.";
                }
                else
                {
                    stat.CurrentNeed = StockNeed.NotSure;
                    stat.NeededAmount = latestDecrease.Amount;
                    stat.Note = $"En son {latestDecrease.ChangeDate.ToLocalTime().Date.ToString("dd MMMM yyyy")} tarihinde {latestDecrease.Amount} kadar tüketilmiş. Alınması gerekiyor mu emin değilim.";
                }
            }
        }
    }
}

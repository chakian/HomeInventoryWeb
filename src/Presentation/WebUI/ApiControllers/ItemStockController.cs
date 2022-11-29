using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.ApiContracts;

namespace WebUI.ApiControllers
{
    public class ItemStockController : AuthApiControllerBase
    {
        readonly IItemStockService _itemStockService;

        public ItemStockController(IUserSettingService userSettingService,
            IItemStockService itemStockService)
            : base(userSettingService)
        {
            _itemStockService = itemStockService;
        }

        [HttpGet]
        public ActionResult<ItemStockGetResponseContract> GetItemStocks(int id = 0, int categoryId = 0)
        {
            var response = new ItemStockGetResponseContract();

            List<ItemStockEntity> itemStockEntities;

            if (id > 0)
            {
                var stockEntity = _itemStockService.GetSingleItemStock(new HomeInv.Common.ServiceContracts.ItemStock.GetSingleItemStockRequest()
                {
                    ItemStockId = id,
                    HomeId = DefaultHomeId,
                    RequestUserId = UserId
                }).Stock;
                if (stockEntity?.Id > 0)
                {
                    itemStockEntities = new List<ItemStockEntity>() { stockEntity };
                }
                else
                {
                    return NotFound("Yok boyle bi stok!");
                }
            }
            else
            {
                itemStockEntities = _itemStockService.GetItemStocksByFilter(new HomeInv.Common.ServiceContracts.ItemStock.GetItemStocksByFilterRequest()
                {
                    HomeId = DefaultHomeId,
                    CategoryId = categoryId,
                    //AreaId = areaId,
                    RequestUserId = UserId
                }).ItemStocks;
            }

            if (itemStockEntities?.Count > 0)
            {
                response.ItemStocks = new List<ItemStockGetResponseContract.ItemStockDefinition>();
                foreach (var item in itemStockEntities)
                {
                    response.ItemStocks.Add(new ItemStockGetResponseContract.ItemStockDefinition()
                    {
                        Id = item.Id,
                        ItemDefinitionId = item.ItemDefinitionId,
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription,
                        AreaId = item.AreaId,
                        AreaName = item.AreaName,
                        RemainingAmount = item.Quantity,
                        ExpirationDate = item.ExpirationDate,
                        SizeUnitId = item.SizeUnitId,
                        SizeUnitName = item.SizeUnitName
                    });
                }
            }
            else
            {
                return NotFound();
            }

            return response;
        }


        // GET: api/ItemStocks/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ItemStock>> GetItemStocks(int id)
        //{
        //    var u = User;
        //    var itemStocks = await _context.ItemStocks.FindAsync(id);

        //    if (itemStocks == null)
        //    {
        //        return NotFound();
        //    }

        //    return itemStocks;
        //}

        // PUT: api/ItemStocks/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemStocks(int id, ItemStock itemStock)
        {
            if (id != itemStock.Id)
            {
                return BadRequest(new { error = "bu ne bicim id" });
            }

            //_context.Entry(itemStock).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemStocksExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ItemStocks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ItemStock>> PostItemStocks(ItemStock itemStock)
        {
            //_context.ItemStocks.Add(itemStock);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemStocks", new { id = itemStock.Id }, itemStock);
        }

        // DELETE: api/ItemStocks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemStock>> DeleteItemStocks(int id)
        {
            //var itemStock = await _context.ItemStocks.FindAsync(id);
            var itemStock = new ItemStock();
            if (itemStock == null)
            {
                return NotFound(new { error = "i ih. yok. bulamadim" });
            }

            //_context.ItemStocks.Remove(itemStock);
            //await _context.SaveChangesAsync();

            return itemStock;
        }

        private bool ItemStocksExists(int id)
        {
            return false;
            //return _context.ItemStocks.Any(e => e.Id == id);
        }
    }
}

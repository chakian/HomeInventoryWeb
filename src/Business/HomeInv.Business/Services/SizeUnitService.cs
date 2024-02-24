using HomeInv.Common.Constants;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.SizeUnit;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HomeInv.Business.Services
{
    public class SizeUnitService : ServiceBase<SizeUnit, SizeUnitEntity>, ISizeUnitService<SizeUnit>
    {
        private readonly IMemoryCache _memoryCache;
        public SizeUnitService(HomeInventoryDbContext context, IMemoryCache memoryCache) : base(context)
        {
            _memoryCache = memoryCache;
        }

        public override SizeUnitEntity ConvertDboToEntity(SizeUnit dbo)
        {
            var sizeUnit = ConvertBaseDboToEntityBase(dbo);
            sizeUnit.Name = dbo.Name;
            sizeUnit.Description = dbo.Description;
            sizeUnit.IsBaseUnit = dbo.IsBaseUnit;
            sizeUnit.ConversionMultiplierToBase = dbo.ConversionMultiplierToBase;
            return sizeUnit;
        }

        public async Task<GetAllSizesResponse> GetAllSizesAsync(GetAllSizesRequest request, CancellationToken ct)
        {
            var response = new GetAllSizesResponse();

            var allSizes = new List<SizeUnit>();

            if (!_memoryCache.TryGetValue(CacheKeys.ALL_SIZE_UNITS, out allSizes))
            {
                allSizes = await context.SizeUnits.ToListAsync(ct);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                _memoryCache.Set(CacheKeys.ALL_SIZE_UNITS, allSizes, cacheEntryOptions);
            }

            response.SizeUnits = allSizes.Select(size => ConvertDboToEntity(size)).ToList();

            return response;
        }
    }
}

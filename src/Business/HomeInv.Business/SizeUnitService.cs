using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.SizeUnit;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;

namespace HomeInv.Business
{
    public class SizeUnitService : ServiceBase<SizeUnit, SizeUnitEntity>, ISizeUnitService<SizeUnit>
    {
        public SizeUnitService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override SizeUnitEntity ConvertDboToEntity(SizeUnit dbo)
        {
            throw new NotImplementedException();
        }

        public GetAllSizesResponse GetAllSizes(GetAllSizesRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

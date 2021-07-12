using HomeInv.Business.Base;
using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Area;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeInv.Business
{
    public class AreaService : AuditableServiceBase<Area, AreaEntity>, IAreaService<Area>
    {
        public AreaService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override AreaEntity ConvertDboToEntity(Area dbo)
        {
            throw new NotImplementedException();
        }

        public CreateAreaResponse CreateArea(CreateAreaRequest request)
        {
            throw new NotImplementedException();
        }

        public GetAreasOfHomeResponse GetAreasOfHome(GetAreasOfHomeRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

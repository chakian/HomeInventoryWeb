using HomeInv.Common.Entities;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts.Area;
using HomeInv.Persistence;
using HomeInv.Persistence.Dbo;
using System.Collections.Generic;
using System.Linq;

namespace HomeInv.Business.Services
{
    public class AreaService : AuditableServiceBase<Area, AreaEntity>, IAreaService<Area>
    {
        public AreaService(HomeInventoryDbContext _context) : base(_context)
        {
        }

        public override AreaEntity ConvertDboToEntity(Area dbo)
        {
            return new AreaEntity()
            {
                Id = dbo.Id,
                HomeId = dbo.HomeId,
                Name = dbo.Name,
                Description = dbo.Description
            };
        }

        public CreateAreaResponse CreateArea(CreateAreaRequest request)
        {
            CreateAreaResponse response = new CreateAreaResponse();
            Area area = CreateNewAuditableObject(request);
            area.HomeId = request.AreaEntity.HomeId;
            area.Name = request.AreaEntity.Name;
            area.Description = request.AreaEntity.Description;

            context.Areas.Add(area);
            //context.Entry(area).State = EntityState.Added;
            context.SaveChanges();

            response.AreaEntity = ConvertDboToEntity(area);
            return response;
        }

        public GetAreasOfHomeResponse GetAreasOfHome(GetAreasOfHomeRequest request)
        {
            var response = new GetAreasOfHomeResponse();
            response.Areas = new List<AreaEntity>();

            var areas = context.Areas.Where(area => area.HomeId == request.HomeId);
            foreach (var area in areas)
            {
                response.Areas.Add(ConvertDboToEntity(area));
            }

            return response;
        }
    }
}

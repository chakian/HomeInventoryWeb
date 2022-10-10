using Microsoft.EntityFrameworkCore;

namespace HomeInv.Persistence
{
    public class DataCleanup
    {
        public void DeleteAllData(HomeInventoryDbContext dbContext)
        {
            dbContext.Database.ExecuteSqlRaw("DELETE FROM ItemStocks");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Items");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Categories");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM HomeUsers");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Homes");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM AreaUsers");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Areas");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM AspNetUsers");
        }
    }
}

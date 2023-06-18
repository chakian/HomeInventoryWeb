using HomeInv.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace HomeInv.Business.Handlers.Tests.Base
{
    public abstract class TestBase
    {
        protected readonly HomeInventoryDbContext _context;

        public TestBase()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<HomeInventoryDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .UseInternalServiceProvider(serviceProvider)
                   .ConfigureWarnings(warn =>
                   {
                       warn.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                   });

            /*
             * This exception can be suppressed or logged by passing event ID 'InMemoryEventId.TransactionIgnoredWarning' 
             * to the 'ConfigureWarnings' method in 'DbContext.OnConfiguring' or 'AddDbContext'.'
             */

            var dbContextOptions = builder.Options;
            _context= new HomeInventoryDbContext(dbContextOptions);
        }
    }
}

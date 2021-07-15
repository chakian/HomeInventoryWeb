using HomeInv.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace WebUI.Base
{
    [Authorize]
    public class BaseAuthenticatedPageModel<T> : BasePageModel<T>
    {
        public BaseAuthenticatedPageModel(ILogger<T> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }
    }
}

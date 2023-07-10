using HomeInv.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers;

[Authorize]
public class AuthenticatedControllerBase : ControllerBase
{
    private readonly HomeInventoryDbContext _dbContext;
    protected readonly string _userId;
    protected readonly int _defaultHomeId;
    protected readonly int _defaultAreaId;

    public AuthenticatedControllerBase(HomeInventoryDbContext dbContext)
    {
        _dbContext = dbContext;

        _userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        _defaultHomeId = _dbContext.UserSettings.SingleOrDefault(setting => setting.UserId == _userId)?.DefaultHomeId ?? -1;
        _defaultAreaId = _dbContext.Areas.FirstOrDefault(a => a.HomeId == _defaultHomeId)?.Id ?? -1;
    }
}

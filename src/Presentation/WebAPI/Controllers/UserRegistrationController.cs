using HomeInv.Persistence.Dbo;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserRegistrationController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserRegistrationController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserRegistrationDto userRegistration)
    {
        var user = userRegistration.Adapt<User>();
        if (string.IsNullOrEmpty(user.UserName)) user.UserName = user.Email;
        var userRegistrationResult = await _userManager.CreateAsync(user, userRegistration.Password);
        return !userRegistrationResult.Succeeded ? new BadRequestObjectResult(userRegistrationResult) : StatusCode(201);
    }
}

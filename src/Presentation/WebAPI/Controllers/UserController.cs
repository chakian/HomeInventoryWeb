using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.ServiceContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Models.User;

namespace WebAPI.Controllers
{
    public class UserController : Controller
    {
        readonly IUserService userService;
        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginModel loginModel = new LoginModel();
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            // TODO: Validations

            LoginUserRequest request = new LoginUserRequest()
            {
                User = new HomeInv.Common.Entities.HIUser()
                {
                    Email = loginModel.Email,
                    Password = loginModel.Password
                }
            };

            var response = userService.LogIn(request);

            if (response.IsSuccessful)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, response.User.Email),
                    //new Claim("FullName", user.FullName),
                    //new Claim(ClaimTypes.Role, "Administrator"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = loginModel.RememberMe,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    IssuedUtc = DateTimeOffset.UtcNow,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                //_logger.LogInformation("User {Email} logged in at {Time}.", user.Email, DateTime.UtcNow);

                return RedirectToAction("Index", "Home");
            }

            string errorMessage = "";
            response.Result.Messages.ForEach(m =>
            {
                errorMessage += m.Text + "\n";
            });
            ModelState.AddModelError(string.Empty, errorMessage);

            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterModel registerModel = new RegisterModel();
            return View(registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            // TODO: Validations

            RegisterUserRequest request = new RegisterUserRequest()
            {
                User = new HomeInv.Common.Entities.HIUser()
                {
                    Email = registerModel.Email,
                    Password = registerModel.Password
                }
            };
            var response = await userService.Register(request);

            if (response.IsSuccessful)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(registerModel);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}

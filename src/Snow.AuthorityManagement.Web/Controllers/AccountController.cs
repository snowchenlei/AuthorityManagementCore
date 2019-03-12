using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Core.Dto.User;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.IService.Authorization;

namespace Snow.AuthorityManagement.Web.Controllers
{
    public class AccountController : BaseController
    {
        private IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            UserLoginOutput output;
            try
            {
                output = await _userService.LoginAsync(input);
            }
            catch (UserFriendlyException ue)
            {
                ModelState.AddModelError("UserName", ue.Message);
                return View(input);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, output.Name),
                new Claim("UserName", output.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties authProperties = new AuthenticationProperties();
            if (input.RememberMe)
            {
                authProperties.IsPersistent = true;
            }
            else
            {
                authProperties.ExpiresUtc = DateTime.UtcNow.AddMinutes(20);
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
            return Redirect("/Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Account/Login");
        }
    }
}
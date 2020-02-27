using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Anc;
using Anc.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Users;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Web.Models.Account;

namespace Snow.AuthorityManagement.Web.Controllers
{
    public class AccountController : BaseController
    {
        private IUserAppService _userService;

        public AccountController(IUserAppService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            UserLoginOutput output;
            try
            {   // TODO:获取角色要改
                output = await _userService.LoginAsync(input.UserName, input.Password);
            }
            catch (UserFriendlyException ue)
            {
                ModelState.AddModelError("UserName", ue.Message);
                return View(input);
            }

            var claims = new List<Claim>
            {
                new Claim(AncClaimTypes.UserId, output.ID.ToString()),
                new Claim(AncClaimTypes.UserName, output.UserName)
            };
            foreach (string roleName in output.RoleNames)
            {
                claims.Add(new Claim(AncClaimTypes.Role, roleName));
            }

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

            HttpContext.Session.Set<UserLoginOutput>("LoginUser", output);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
            return Redirect("/Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("LoginUser");
            return Redirect("/Account/Login");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cl.AuthorityManagement.Common;
using Cl.AuthorityManagement.Common.Encryption;
using Cl.AuthorityManagement.Entity;
using Cl.AuthorityManagement.Enum;
using Cl.AuthorityManagement.IServices;
using Cl.AuthorityManagement.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Cl.AuthorityManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("any")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserInfoServices UserInfoServices = null;
        private readonly IDistributedCache Cache = null;
        public AccountController(
            IUserInfoServices userInfoServices,
            IDistributedCache cache)
        {
            UserInfoServices = userInfoServices;
            Cache = cache;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]JObject jobj)
        {
            //if (!string.Equals(HttpContext.Session.Get<string>("verCode")
            //    , login.VerifyCode, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    return BadRequest(new Result
            //    {
            //         State = 0,
            //         Message = "验证码错误"
            //    });
            //}
            //string s = jobj["fsfsf"].ToString();
            string username = jobj["username"]?.ToString(),
                password = jobj["password"]?.ToString();
            
            if (IsValidUserAndPasswordCombination(username, password))
            {
                return BadRequest(new Result
                {
                    State = 0,
                    Message = "用户名或密码不能为空"
                });
            }

            password = Md5Encryption.Encrypt(Md5Encryption.Encrypt(password, Md5EncryptionType.Strong));
            UserInfo userInfo = UserInfoServices
                .LoadFirst(entity => entity.UserName == username
                    && entity.Password == password);

            if (userInfo == null)
            {
                return BadRequest(new Result
                {
                    State = 0,
                    Message = "用户名或密码不正确"
                });
            }
            if (userInfo.IsCanUse == false)
            {
                return BadRequest(new Result
                {
                    State = 0,
                    Message = "当前用户不可用"
                });
            }
            
            string token = GenerateToken(username);
            Cache.SetString(token, userInfo.UserName);
            return Ok(new Result<string>
            {
                State = 1,
                Message = "登陆成功",
                Data = token
            });
        }

        private bool IsValidUserAndPasswordCombination(string username, string password)
        {
            return string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password);
            return !string.IsNullOrEmpty(username) && username == password;
        }

        /// <summary>
        /// 生产Token
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        private string GenerateToken(string username)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long for HmacSha256")),
                                             SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        [HttpGet]
        public IActionResult GetVerifyCodeImage()
        {
            string code = string.Empty;
            byte[] buffer = VerifyCode.Create(4, out code);
            return File(buffer, @"image/jpeg");
        }
    }
}
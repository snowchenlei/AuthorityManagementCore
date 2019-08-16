using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Common.Conversion;

namespace Snow.AuthorityManagement.Web.Authorization
{
    public class AuthorizationFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public AuthorizationFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserLoginOutput GetLoginUserInfo()
        {
            return Serialization.DeserializeObject<UserLoginOutput>(_session.Get("LoginUser"));
            //return new UserLoginOutput();
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    //这里通过 HttpContext.User.Claims 可以将我们在Login这个Action中存储到cookie中的所有
            //    //claims键值对都读出来，比如我们刚才定义的UserName的值Wangdacui就在这里读取出来了
            //    var userName = HttpContext.User.Claims.First().Value;
            //}

            //string key = "loginUser";
            //LoginUserInfo loginUser = CallContext.GetData(key) as LoginUserInfo;
            //if (loginUser == null)
            //{
            //    loginUser = GetUserInfo(HttpContext.Current);

            //    CallContext.SetData(key, loginUser);
            //}
            //return loginUser;
        }
    }
}
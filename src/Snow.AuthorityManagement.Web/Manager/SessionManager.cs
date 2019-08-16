using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Core.Model;

namespace Snow.AuthorityManagement.Web.Manager
{
    public class SessionManager
    {
        private readonly ISession _session;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public UserLoginOutput UserLoginOutput
        {
            get
            {
                string session = _session.GetString("LoginUser");
                if (String.IsNullOrEmpty(session))
                {
                    throw new AncAuthorizationException("请登陆");
                }
                UserLoginOutput user = Serialization.DeserializeObject<UserLoginOutput>(session);
                return user;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Anc.Security.Claims.Anc.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Anc.AspNetCore.Security.Claims
{
    //TODO:注入到此实现
    public class HttpContextCurrentPrincipalAccessor : ThreadCurrentPrincipalAccessor
    {
        public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
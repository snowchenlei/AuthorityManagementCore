using System.Security.Claims;
using Anc.DependencyInjection;
using Anc.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Anc.AspNetCore.Security.Claims
{
    // TODO:子类无需注入实现
    public class HttpContextCurrentPrincipalAccessor : ThreadCurrentPrincipalAccessor, ISingletonDependency
    {
        public override ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? base.Principal;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
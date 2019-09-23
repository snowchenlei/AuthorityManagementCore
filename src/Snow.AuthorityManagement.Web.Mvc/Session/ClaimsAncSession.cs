using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Snow.AuthorityManagement.Web.Session
{
    public class ClaimsAncSession : IAncSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsAncSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim?.Value))
                {
                    return null;
                }

                int userId;
                if (!int.TryParse(userIdClaim.Value, out userId))
                {
                    return null;
                }

                return userId;
            }
        }
    }
}
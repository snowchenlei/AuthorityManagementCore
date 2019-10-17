using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Anc.DependencyInjection;

namespace Anc.Runtime.Session
{
    public class ClaimsAncSession : IAncSession, ISingletonDependency
    {
        public virtual bool IsAuthenticated => UserId.HasValue;

        protected IPrincipalAccessor PrincipalAccessor { get; }

        public ClaimsAncSession(IPrincipalAccessor principalAccessor)
        {
            PrincipalAccessor = principalAccessor;
        }

        public int? UserId
        {
            get
            {
                var userIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
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
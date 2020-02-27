using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Anc;
using JetBrains.Annotations;

namespace System.Security.Principal
{
    public static class ClaimsIdentityExtensions
    {
        public static Claim FirstOrDefault(this ClaimsPrincipal principal, Func<Claim, bool> predicate)
        {
            Check.NotNull(principal, nameof(principal));

            var userIdOrNull = principal.Claims?.FirstOrDefault(predicate);
            return userIdOrNull;
        }

        public static Claim FirstOrDefault(this ClaimsIdentity identity, Func<Claim, bool> predicate)
        {
            Check.NotNull(identity, nameof(identity));

            var userIdOrNull = identity?.Claims?.FirstOrDefault(predicate);
            return userIdOrNull;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Anc.Security.Claims
{
    public interface ICurrentPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}
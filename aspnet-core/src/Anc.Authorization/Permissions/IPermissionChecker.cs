using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionChecker
    {
        Task<bool> IsGrantedAsync([NotNull]string name);

        Task<bool> IsGrantedAsync([CanBeNull] ClaimsPrincipal claimsPrincipal, [NotNull]string name);
    }
}
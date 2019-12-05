using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionStore
    {
        Task<bool> IsGrantedAsync(
            string name,
            string providerName,
            string providerKey
        );
    }
}
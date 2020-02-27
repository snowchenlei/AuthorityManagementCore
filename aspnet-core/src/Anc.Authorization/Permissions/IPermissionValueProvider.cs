using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionValueProvider
    {
        string Name { get; }

        //TODO: Rename to GetResult? (CheckAsync throws exception by naming convention)
        Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);
    }
}
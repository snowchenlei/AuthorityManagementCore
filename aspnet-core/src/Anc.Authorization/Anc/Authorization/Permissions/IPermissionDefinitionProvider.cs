using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionDefinitionProvider
    {
        void Define(IPermissionDefinitionContext context);
    }
}
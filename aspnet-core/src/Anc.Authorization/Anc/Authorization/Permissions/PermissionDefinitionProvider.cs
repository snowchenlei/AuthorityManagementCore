using System;
using System.Collections.Generic;
using System.Text;
using Anc.DependencyInjection;

namespace Anc.Authorization.Permissions
{
    public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ITransientDependency
    {
        public abstract void Define(IPermissionDefinitionContext context);
    }
}
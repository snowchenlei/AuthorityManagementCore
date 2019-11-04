using System;
using System.Collections.Generic;
using System.Text;
using Anc.Authorization.Permissions;
using Anc.Collections;

namespace Anc.Authorization.Permissions
{
    public class AncPermissionOptions
    {
        public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; }

        public ITypeList<IPermissionValueProvider> ValueProviders { get; }

        public AncPermissionOptions()
        {
            DefinitionProviders = new TypeList<IPermissionDefinitionProvider>();
            ValueProviders = new TypeList<IPermissionValueProvider>();
        }
    }
}
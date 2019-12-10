using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Authorization.Permissions
{
    public interface IPermissionValueProviderManager
    {
        IReadOnlyList<IPermissionValueProvider> ValueProviders { get; }
    }
}
using Anc.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Authorization
{
    /// <summary>
    /// This is the main interface to define permissions for an application. Implement it to define
    /// permissions for your module.
    /// </summary>
    public interface IAuthorizationProvider : ITransientDependency
    {
        /// <summary>
        /// This method is called once on application startup to allow to define permissions.
        /// </summary>
        /// <param name="context">Permission definition context</param>
        void SetPermissions(IPermissionDefinitionContext context);
    }
}
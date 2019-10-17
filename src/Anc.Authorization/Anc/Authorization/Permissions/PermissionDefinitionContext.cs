using System;
using System.Collections.Generic;
using System.Text;
using Anc.Authorization.Permissions;

namespace Anc.Authorization.Anc.Authorization.Permissions
{
    public class PermissionDefinitionContext : IPermissionDefinitionContext
    {
        internal Dictionary<string, PermissionGroupDefinition> Groups { get; }

        internal PermissionDefinitionContext()
        {
            Groups = new Dictionary<string, PermissionGroupDefinition>();
        }

        public virtual PermissionGroupDefinition AddGroup(
            string name,
            string displayName = null)
        {
            Check.NotNull(name, nameof(name));

            if (Groups.ContainsKey(name))
            {
                throw new AncException($"There is already an existing permission group with name: {name}");
            }

            return Groups[name] = new PermissionGroupDefinition(name, displayName);
        }

        public virtual PermissionGroupDefinition GetGroupOrNull(string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Groups.ContainsKey(name))
            {
                return null;
            }

            return Groups[name];
        }

        public virtual void RemoveGroup(string name)
        {
            Check.NotNull(name, nameof(name));

            if (!Groups.ContainsKey(name))
            {
                throw new AncException($"Not found permission group with name: {name}");
            }

            Groups.Remove(name);
        }
    }
}
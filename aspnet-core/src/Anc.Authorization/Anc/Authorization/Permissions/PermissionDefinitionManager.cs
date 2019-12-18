using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Anc.Authorization.Anc.Authorization.Permissions;
using Anc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Anc.Authorization.Permissions
{
    public class PermissionDefinitionManager : IPermissionDefinitionManager, ISingletonDependency
    {
        protected IDictionary<string, PermissionDefinition> PermissionTreeDefinitions => _lazyPermissionTreeDefinitions.Value;
        private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionTreeDefinitions;

        protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;
        private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;

        protected AncPermissionOptions Options { get; }

        private readonly IServiceProvider _serviceProvider;
        private readonly IEnumerable<IPermissionDefinitionProvider> PermissionDefinitionProviders;

        public PermissionDefinitionManager(
            IOptions<AncPermissionOptions> options,
            IServiceProvider serviceProvider,
            IEnumerable<IPermissionDefinitionProvider> permissionDefinitionProviders)
        {
            _serviceProvider = serviceProvider;
            PermissionDefinitionProviders = permissionDefinitionProviders;
            Options = options.Value;

            _lazyPermissionDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(
                CreatePermissionDefinitions,
                isThreadSafe: true
            );

            _lazyPermissionTreeDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(
                CreatePermissionTreeDefinitions,
                isThreadSafe: true
            );
        }

        public virtual PermissionDefinition Get(string name)
        {
            var permission = GetOrNull(name);

            if (permission == null)
            {
                throw new AncException("Undefined permission: " + name);
            }

            return permission;
        }

        public virtual PermissionDefinition GetOrNull(string name)
        {
            Check.NotNull(name, nameof(name));
            if (PermissionDefinitions.GetOrDefault(name) != null)
            {
                return PermissionDefinitions.GetOrDefault(name);
            }
            foreach (KeyValuePair<string, PermissionDefinition> item in PermissionDefinitions)
            {
                GetPermissionDefinition(item.Value.Children, name);
            }

            return PermissionDefinitions.GetOrDefault(name);
        }

        private PermissionDefinition GetPermissionDefinition(IEnumerable<PermissionDefinition> permissionDefinitions, string name)
        {
            foreach (PermissionDefinition item in permissionDefinitions)
            {
                if (item.Name == name)
                {
                    return item;
                }
                if (item.Children.Any(p => p.Name == name))
                {
                    return item.Children.First(p => p.Name == name);
                }
                else
                {
                    return GetPermissionDefinition(item.Children, name);
                }
            }
            return null;
        }

        public virtual IReadOnlyList<PermissionDefinition> GetPermissions()
        {
            return PermissionDefinitions.Values.ToImmutableList();
        }

        protected virtual Dictionary<string, PermissionDefinition> CreatePermissionDefinitions()
        {
            var permissions = new Dictionary<string, PermissionDefinition>();

            foreach (var treeDefinition in PermissionTreeDefinitions.Values)
            {
                AddPermissionToDictionaryRecursively(permissions, treeDefinition);
            }

            return permissions;
        }

        protected virtual void AddPermissionToDictionaryRecursively(
            Dictionary<string, PermissionDefinition> permissions,
            PermissionDefinition permission)
        {
            if (permissions.ContainsKey(permission.Name))
            {
                throw new AncException("Duplicate permission name: " + permission.Name);
            }

            permissions[permission.Name] = permission;

            foreach (var child in permission.Children)
            {
                AddPermissionToDictionaryRecursively(permissions, child);
            }
        }

        protected virtual Dictionary<string, PermissionDefinition> CreatePermissionTreeDefinitions()
        {
            var context = new PermissionDefinitionContext();

            foreach (var provider in PermissionDefinitionProviders)
            {
                provider.Define(context);
            }

            return context.PermissionDefinitions;
        }
    }
}
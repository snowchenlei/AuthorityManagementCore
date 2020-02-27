﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Anc.Authorization.Permissions
{
    public class PermissionGroupDefinition //TODO: Consider to make possible a group have sub groups
    {
        /// <summary>
        /// Unique name of the group.
        /// </summary>
        public string Name { get; }

        public Dictionary<string, object> Properties { get; }

        public string DisplayName
        {
            get => _displayName;
            set => _displayName = Check.NotNull(value, nameof(value));
        }

        private string _displayName;

        public IReadOnlyList<PermissionDefinition> Permissions => _permissions.ToImmutableList();
        private readonly List<PermissionDefinition> _permissions;

        /// <summary>
        /// Gets/sets a key-value on the <see cref="Properties"/>.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <returns>
        /// Returns the value in the <see cref="Properties"/> dictionary by given <see cref="name"/>.
        /// Returns null if given <see cref="name"/> is not present in the <see cref="Properties"/> dictionary.
        /// </returns>
        public object this[string name]
        {
            get => Properties.GetOrDefault(name);
            set => Properties[name] = value;
        }

        protected internal PermissionGroupDefinition(
            string name,
            string displayName = null)
        {
            Name = name;
            DisplayName = displayName;

            Properties = new Dictionary<string, object>();
            _permissions = new List<PermissionDefinition>();
        }

        public virtual PermissionDefinition AddPermission(
            string name,
            string displayName = null)
        {
            var permission = new PermissionDefinition(name, displayName);

            _permissions.Add(permission);

            return permission;
        }

        public virtual List<PermissionDefinition> GetPermissionsWithChildren()
        {
            var permissions = new List<PermissionDefinition>();

            foreach (var permission in _permissions)
            {
                AddPermissionToListRecursively(permissions, permission);
            }

            return permissions;
        }

        private void AddPermissionToListRecursively(List<PermissionDefinition> permissions, PermissionDefinition permission)
        {
            permissions.Add(permission);

            foreach (var child in permission.Children)
            {
                AddPermissionToListRecursively(permissions, child);
            }
        }

        public override string ToString()
        {
            return $"[{nameof(PermissionGroupDefinition)} {Name}]";
        }
    }
}
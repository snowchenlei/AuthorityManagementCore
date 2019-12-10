using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Snow.AuthorityManagement.Core.Model
{
    public class AncPermission
    {
        /// <summary>
        /// Parent of this permission if one exists.
        /// If set, this permission can be granted only if parent is granted.
        /// </summary>
        public AncPermission Parent { get; private set; }

        /// <summary>
        /// Unique name of the permission.
        /// This is the key name to grant permissions.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Display name of the permission.
        /// This can be used to show permission to the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// A brief description for this permission.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of child permissions. A child permission can be granted only if parent is granted.
        /// </summary>
        public IReadOnlyList<AncPermission> Children => _children.ToImmutableList();

        private readonly List<AncPermission> _children;

        /// <summary>
        /// Creates a new AncPermission.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        /// <param name="displayName">Display name of the permission</param>
        /// <param name="description">A brief description for this permission</param>
        public AncPermission(
            string name,
            string displayName = null,
            string description = null)
        {
            Name = name ?? throw new ArgumentNullException("name");
            DisplayName = displayName;
            Description = description;

            _children = new List<AncPermission>();
        }

        /// <summary>
        /// Adds a child permission.
        /// A child permission can be granted only if parent is granted.
        /// </summary>
        /// <returns>Returns newly created child permission</returns>
        public AncPermission CreateChildPermission(
            string name,
            string displayName = null,
            string description = null)
        {
            var permission = new AncPermission(name, displayName, description) { Parent = this };
            _children.Add(permission);
            return permission;
        }

        public void RemoveChildPermission(string name)
        {
            _children.RemoveAll(p => p.Name == name);
        }

        public override string ToString()
        {
            return string.Format("[AncPermission: {0}]", Name);
        }
    }
}
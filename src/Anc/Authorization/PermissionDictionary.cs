using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anc.Authorization
{
    public class PermissionDictionary : Dictionary<string, AncPermission>
    {
        /// <summary>
        /// Adds all child permissions of current permissions recursively.
        /// </summary>
        public void AddAllPermissions()
        {
            foreach (var permission in Values.ToList())
            {
                AddPermissionRecursively(permission);
            }
        }

        /// <summary>
        /// Adds a permission and it's all child permissions to dictionary.
        /// </summary>
        /// <param name="permission">Permission to be added</param>
        private void AddPermissionRecursively(AncPermission permission)
        {
            //Prevent multiple adding of same named permission.
            AncPermission existingPermission;
            if (TryGetValue(permission.Name, out existingPermission))
            {
                if (existingPermission != permission)
                {
                    throw new AncInitializationException("Duplicate permission name detected for " + permission.Name);
                }
            }
            else
            {
                this[permission.Name] = permission;
            }

            //Add child permissions (recursive call)
            foreach (var childPermission in permission.Children)
            {
                AddPermissionRecursively(childPermission);
            }
        }
    }
}
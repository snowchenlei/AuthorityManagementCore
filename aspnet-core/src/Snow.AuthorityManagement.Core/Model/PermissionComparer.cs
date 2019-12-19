using System;
using System.Collections.Generic;
using System.Text;
using Anc.Domain.Entities;
using Snow.AuthorityManagement.Core.Authorization.Permissions;

namespace Snow.AuthorityManagement.Core.Model
{
    public class PermissionComparer : IEqualityComparer<AncPermission>
    {
        public bool Equals(AncPermission x, AncPermission y)
        {
            //this非空，obj如果为空，则返回false
            if (object.ReferenceEquals(x, null))
            {
                return false;
            }
            //this非空，obj如果为空，则返回false
            if (object.ReferenceEquals(y, null))
            {
                return false;
            }
            //如果为同一对象，必然相等
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(AncPermission obj)
        {
            int hashScore = obj.Name.GetHashCode();
            return hashScore;
        }
    }
}
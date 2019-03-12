using System;
using System.Collections.Generic;
using System.Text;
using Snow.AuthorityManagement.Core.Entities.Authorization;

namespace Snow.AuthorityManagement.Core.Model
{
    public class PermissionComparer : IEqualityComparer<Permission>
    {
        public bool Equals(Permission x, Permission y)
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

        public int GetHashCode(Permission obj)
        {
            int hashScore = obj.Name.GetHashCode();
            return hashScore;
        }
    }
}
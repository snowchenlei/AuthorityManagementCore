using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Authorization.UserRoles
{
    public class UserRole
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
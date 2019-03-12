using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Entities.Authorization
{
    public class UserRole
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
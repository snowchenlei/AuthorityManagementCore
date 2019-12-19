using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Model
{
    public class LoginUserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPermissions { get; set; }
    }
}
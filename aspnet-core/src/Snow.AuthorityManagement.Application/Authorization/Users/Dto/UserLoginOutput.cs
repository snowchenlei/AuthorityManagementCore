using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Dto
{
    public class UserLoginOutput
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }

        public string[] RoleNames { get; set; }
    }
}
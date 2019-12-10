using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Authorization
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Name = "Admin";
            public const string DisplayName = "系统管理员";
        }

        public static class Tenants
        {
            public const string Admin = "Admin";
        }
    }
}
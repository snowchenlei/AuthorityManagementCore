using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Authorization
{
    public class PermissionNames
    {
        #region 用户管理

        public const string Pages_Users = "Pages.Users";
        public const string Pages_Users_Query = "Pages.Users.Query";
        public const string Pages_Users_Create = "Pages.Users.Create";
        public const string Pages_Users_Edit = "Pages.Users.Edit";
        public const string Pages_Users_Reset = "Pages.Users.Reset";
        public const string Pages_Users_Delete = "Pages.Users.Delete";
        public const string Pages_Users_BatchDelete = "Pages.Users.BatchDelete";

        #endregion 用户管理

        #region 角色管理

        public const string Pages_Roles = "Pages.Roles";
        public const string Pages_Roles_Query = "Pages.Roles.Query";
        public const string Pages_Roles_Create = "Pages.Roles.Create";
        public const string Pages_Roles_Edit = "Pages.Roles.Edit";
        public const string Pages_Roles_Delete = "Pages.Roles.Delete";
        public const string Pages_Roles_BatchDelete = "Pages.Roles.BatchDelete";

        #endregion 角色管理
    }
}
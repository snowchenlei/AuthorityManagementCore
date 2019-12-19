using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Core
{
    public class PermissionNames
    {
        public const string Pages = "Pages";
        public const string Pages_Administration = "Pages.Administration";

        #region 用户管理

        public const string Pages_Administration_Users = "Pages.Administration.Users";
        public const string Pages_Administration_Users_Query = "Pages.Administration.Users.Query";
        public const string Pages_Administration_Users_Create = "Pages.Administration.Users.Create";
        public const string Pages_Administration_Users_Edit = "Pages.Administration.Users.Edit";
        public const string Pages_Administration_Users_Reset = "Pages.Administration.Users.Reset";
        public const string Pages_Administration_Users_Delete = "Pages.Administration.Users.Delete";
        public const string Pages_Administration_Users_BatchDelete = "Pages.Administration.Users.BatchDelete";

        #endregion 用户管理

        #region 角色管理

        public const string Pages_Administration_Roles = "Pages.Administration.Roles";
        public const string Pages_Administration_Roles_Query = "Pages.Administration.Roles.Query";
        public const string Pages_Administration_Roles_Create = "Pages.Administration.Roles.Create";
        public const string Pages_Administration_Roles_Edit = "Pages.Administration.Roles.Edit";
        public const string Pages_Administration_Roles_Delete = "Pages.Administration.Roles.Delete";
        public const string Pages_Administration_Roles_BatchDelete = "Pages.Administration.Roles.BatchDelete";

        #endregion 角色管理

        #region 菜单管理

        public const string Pages_Administration_Menus = "Pages.Administration.Menus";
        public const string Pages_Administration_Menus_Query = "Pages.Administration.Menus.Query";
        public const string Pages_Administration_Menus_Create = "Pages.Administration.Menus.Create";
        public const string Pages_Administration_Menus_Edit = "Pages.Administration.Menus.Edit";
        public const string Pages_Administration_Menus_Delete = "Pages.Administration.Menus.Delete";
        public const string Pages_Administration_Menus_BatchDelete = "Pages.Administration.Menus.BatchDelete";

        #endregion 菜单管理
    }
}
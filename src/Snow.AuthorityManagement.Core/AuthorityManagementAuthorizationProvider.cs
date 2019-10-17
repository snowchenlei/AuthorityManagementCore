using System.Linq;
using Anc.Authorization;

namespace Snow.AuthorityManagement.Core
{
    public class AuthorityManagementAuthorizationProvider : IAuthorizationProvider
    {
        public void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull(PermissionNames.Pages) ??
                        context.CreatePermission(PermissionNames.Pages, "Pages");

            AncPermission administration = pages.Children.FirstOrDefault(p => p.Name == PermissionNames.Pages_Administration) ??
                                 pages.CreateChildPermission(PermissionNames.Pages_Administration, "Administration");

            #region 用户管理

            AncPermission userPermission = administration.CreateChildPermission(PermissionNames.Pages_Administration_Users, "用户管理");
            userPermission.CreateChildPermission(PermissionNames.Pages_Administration_Users_Query, "查询");
            userPermission.CreateChildPermission(PermissionNames.Pages_Administration_Users_Create, "创建");
            userPermission.CreateChildPermission(PermissionNames.Pages_Administration_Users_Edit, "修改");
            userPermission.CreateChildPermission(PermissionNames.Pages_Administration_Users_Delete, "删除");
            userPermission.CreateChildPermission(PermissionNames.Pages_Administration_Users_BatchDelete, "批量删除");

            #endregion 用户管理

            #region 角色管理

            AncPermission rolePermission = administration.CreateChildPermission(PermissionNames.Pages_Administration_Roles, "角色管理");
            rolePermission.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Query, "查询");
            rolePermission.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Create, "创建");
            rolePermission.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Edit, "修改");
            rolePermission.CreateChildPermission(PermissionNames.Pages_Administration_Roles_Delete, "删除");
            rolePermission.CreateChildPermission(PermissionNames.Pages_Administration_Roles_BatchDelete, "批量删除");

            #endregion 角色管理

            #region 菜单管理

            AncPermission menuPermission = administration.CreateChildPermission(PermissionNames.Pages_Administration_Menus, "菜单管理");
            menuPermission.CreateChildPermission(PermissionNames.Pages_Administration_Menus_Query, "查询");
            menuPermission.CreateChildPermission(PermissionNames.Pages_Administration_Menus_Create, "创建");
            menuPermission.CreateChildPermission(PermissionNames.Pages_Administration_Menus_Edit, "修改");
            menuPermission.CreateChildPermission(PermissionNames.Pages_Administration_Menus_Delete, "删除");
            menuPermission.CreateChildPermission(PermissionNames.Pages_Administration_Menus_BatchDelete, "批量删除");

            #endregion 菜单管理
        }
    }
}
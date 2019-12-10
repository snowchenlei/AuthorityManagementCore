﻿using System.Linq;
using Anc.Authorization.Permissions;
using Anc.DependencyInjection;

namespace Snow.AuthorityManagement.Core
{
    public class AuthorityManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var pages = context.AddGroup(PermissionNames.Pages_Administration, "Pages_Administration");

            //var pages = context.GetPermissionOrNull(PermissionNames.Pages) ??
            //            context.CreatePermission(PermissionNames.Pages, "Pages");

            //AncPermission administration = pages.Children.FirstOrDefault(p => p.Name == PermissionNames.Pages_Administration) ??
            //                     pages.CreateChildPermission(PermissionNames.Pages_Administration, "Administration");

            #region 用户管理

            PermissionDefinition userPermission = pages.AddPermission(PermissionNames.Pages_Administration_Users, "用户管理");
            userPermission.AddChild(PermissionNames.Pages_Administration_Users_Query, "查询");
            userPermission.AddChild(PermissionNames.Pages_Administration_Users_Create, "创建");
            userPermission.AddChild(PermissionNames.Pages_Administration_Users_Edit, "修改");
            userPermission.AddChild(PermissionNames.Pages_Administration_Users_Delete, "删除");
            userPermission.AddChild(PermissionNames.Pages_Administration_Users_BatchDelete, "批量删除");

            #endregion 用户管理

            #region 角色管理

            PermissionDefinition rolePermission = pages.AddPermission(PermissionNames.Pages_Administration_Roles, "角色管理");
            rolePermission.AddChild(PermissionNames.Pages_Administration_Roles_Query, "查询");
            rolePermission.AddChild(PermissionNames.Pages_Administration_Roles_Create, "创建");
            rolePermission.AddChild(PermissionNames.Pages_Administration_Roles_Edit, "修改");
            rolePermission.AddChild(PermissionNames.Pages_Administration_Roles_Delete, "删除");
            rolePermission.AddChild(PermissionNames.Pages_Administration_Roles_BatchDelete, "批量删除");

            #endregion 角色管理

            #region 菜单管理

            PermissionDefinition menuPermission = pages.AddPermission(PermissionNames.Pages_Administration_Menus, "菜单管理");
            menuPermission.AddChild(PermissionNames.Pages_Administration_Menus_Query, "查询");
            menuPermission.AddChild(PermissionNames.Pages_Administration_Menus_Create, "创建");
            menuPermission.AddChild(PermissionNames.Pages_Administration_Menus_Edit, "修改");
            menuPermission.AddChild(PermissionNames.Pages_Administration_Menus_Delete, "删除");
            menuPermission.AddChild(PermissionNames.Pages_Administration_Menus_BatchDelete, "批量删除");

            #endregion 菜单管理
        }
    }
}
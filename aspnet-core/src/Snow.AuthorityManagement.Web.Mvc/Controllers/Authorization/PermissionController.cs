using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anc.Authorization;
using Anc.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Permissions;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Exception;

namespace Snow.AuthorityManagement.Web.Controllers.Authorization
{
    public class PermissionController : BaseController
    {
        private readonly IPermissionAppService _permissionService;
        private readonly IAncSession _ancSession;

        public PermissionController(IPermissionAppService permissionService, IAncSession ancSession)
        {
            _permissionService = permissionService;
            _ancSession = ancSession;
        }

        public async Task<JsonResult> IsGrantedAsync(string permissionName)
        {
            if (!_ancSession.UserId.HasValue)
            {
                throw new AncAuthorizationException("请登陆");
            }
            bool have = await _permissionService.IsGrantedAsync(permissionName, _ancSession.UserId.Value);
            return Json(new Result<bool>()
            {
                Status = Status.Success,
                Data = have
            });
        }

        public async Task<JsonResult> GetAllPermission()
        {
            List<Permission> permissions = await _permissionService.GetAllPermissionsAsync(_ancSession.UserId.Value);
            return Json(new Result<object>
            {
                Status = Status.Success,
                Data = permissions.Select(p => p.Name).ToArray()
            });
        }
    }
}
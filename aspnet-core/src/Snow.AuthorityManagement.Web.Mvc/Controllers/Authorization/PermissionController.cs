using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anc.Authorization;
using Anc.Users;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Application.Authorization.Permissions;
using Snow.AuthorityManagement.Core;

namespace Snow.AuthorityManagement.Web.Controllers.Authorization
{
    public class PermissionController : BaseController
    {
        private readonly IPermissionAppService _permissionService;
        private readonly ICurrentUser _user;

        public PermissionController(IPermissionAppService permissionService, ICurrentUser user)
        {
            _permissionService = permissionService;
            _user = user;
        }

        public async Task<JsonResult> IsGrantedAsync(string permissionName)
        {
            if (!_user.Id.HasValue)
            {
                throw new AncAuthorizationException("请登陆");
            }
            bool have = await _permissionService.IsGrantedAsync(permissionName, _user.GetId());
            return Json(new Result<bool>()
            {
                Status = Status.Success,
                Data = have
            });
        }

        public async Task<JsonResult> GetAllPermission()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync(_user.GetId());
            return Json(new Result<object>
            {
                Status = Status.Success,
                Data = permissions.Select(p => p.Name).ToArray()
            });
        }
    }
}
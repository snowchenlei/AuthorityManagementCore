using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Dto.Permission;
using Snow.AuthorityManagement.Core.Dto.Role;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Model;
using Snow.AuthorityManagement.IService.Authorization;
using Snow.AuthorityManagement.Web.Authorization;
using Snow.AuthorityManagement.Web.Library;

namespace Snow.AuthorityManagement.Web.Controllers.Authorization
{
    [RBACAuthorize(PermissionNames.Pages_Roles)]
    public class RoleController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly PermissionDefinitionContextBase _context;

        public RoleController(
            IRoleService roleService, IPermissionService permissionService
            //, PermissionDefinitionContextBase context
            , IMapper mapper)
        {
            _roleService = roleService;
            _permissionService = permissionService;
            _context = PermissionDefinitionContextBase.Context;//context;
            _mapper = mapper;
        }

        [RBACAuthorize(PermissionNames.Pages_Roles)]
        [ResponseCache(CacheProfileName = "Header")]
        public ActionResult Index()
        {
            return View();
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Query)]
        public async Task<JsonResult> Load(GetRoleInput input)
        {
            var result = await _roleService.GetPagedAsync(input);
            return Json(new
            {
                total = result.TotalCount,
                rows = result.Items
            });
        }

        [HttpGet]
        [AjaxOnly]
        [RBACAuthorize(PermissionNames.Pages_Roles_Create, PermissionNames.Pages_Roles_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            if (id.HasValue)
            {
                return PartialView(await Edit(id.Value));
            }
            else
            {
                return PartialView(Create());
            }
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Create)]
        private GetRoleForEditOutput Create()
        {
            IReadOnlyList<AncPermission> permissions = _context
                .Permissions.Values.ToImmutableList();
            ICollection<FlatPermissionDto> result = new List<FlatPermissionDto>();
            foreach (AncPermission permission in permissions.Where(p => p.Parent == null))
            {
                result.Add(AddPermission(permission, null));
            }
            return new GetRoleForEditOutput()
            {
                Role = null,
                Permission = Serialization.SerializeObjectCamel(result)
            };
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Edit)]
        private async Task<GetRoleForEditOutput> Edit(int roleId)
        {
            var roleDto = await _roleService.GetForEditAsync(roleId);
            IReadOnlyList<AncPermission> permissions = _context
                .Permissions.Values.ToImmutableList();
            ICollection<FlatPermissionDto> result = new List<FlatPermissionDto>();
            List<Permission> olPermissions = await _permissionService.GetPermissionsByRoleIdAsync(roleId);
            foreach (AncPermission permission in permissions.Where(p => p.Parent == null))
            {
                result.Add(AddPermission(permission, olPermissions));
            }

            return new GetRoleForEditOutput()
            {
                Role = roleDto,
                Permission = Serialization.SerializeObjectCamel(result)
            };
        }

        private FlatPermissionDto AddPermission(AncPermission permission
            , IList<Permission> oldPermissions)
        {
            var flatPermission = _mapper.Map<FlatPermissionDto>(permission);
            if (oldPermissions != null && oldPermissions.Any(op => op.Name == permission.Name))
            {
                flatPermission.IsGranted = true;
            }
            foreach (AncPermission child in permission.Children)
            {
                if (flatPermission.Children == null)
                {
                    flatPermission.Children = new List<FlatPermissionDto>();
                }

                flatPermission.Children.Add(AddPermission(child, oldPermissions));
            }

            return flatPermission;
        }

        [HttpPost]
        [AjaxOnly]
        [RBACAuthorize(PermissionNames.Pages_Roles_Create, PermissionNames.Pages_Roles_Edit)]
        public async Task<ActionResult> CreateOrEdit(CreateOrUpdateRole input)
        {
            if (ModelState.IsValid)
            {
                RoleListDto roleListDto;
                if (input.Role.ID.HasValue)
                {
                    roleListDto = await Edit(input.Role, input.Permission);
                }
                else
                {
                    roleListDto = await Create(input.Role, input.Permission);
                }

                return Json(new Result<RoleListDto>()
                {
                    Status = Status.Success,
                    Message = "操作成功",
                    Data = roleListDto
                });
            }
            else
            {
                IEnumerable<string> errors = ModelStateToArray();
                return Json(new Result
                {
                    Status = Status.Failure,
                    Message = "[" + string.Join(",", errors) + "]"
                });
            }
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Create)]
        public Task<RoleListDto> Create(RoleEditDto input, string permission)
        {
            return _roleService.AddAsync(input, permission);
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Edit)]
        public Task<RoleListDto> Edit(RoleEditDto input, string permission)
        {
            return _roleService.EditAsync(input, permission);
        }

        [RBACAuthorize(PermissionNames.Pages_Roles_Delete)]
        public async Task<JsonResult> Delete(int id)
        {
            if (await _roleService.DeleteAsync(id))
            {
                return Json(new Result()
                {
                    Status = Status.Success,
                    Message = "删除成功"
                });
            }
            return Json(new Result()
            {
                Status = Status.Failure,
                Message = "删除失败"
            });
        }
    }
}
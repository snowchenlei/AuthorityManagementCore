using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Model;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Application.Authorization.Permissions.Dto;
using Anc.Domain.Repositories;
using Anc.Application.Services.Dto;
using Anc.Domain.Uow;
using Anc;
using Anc.Domain.Entities;
using Snow.AuthorityManagement.EntityFrameworkCore;
using Anc.Authorization.Permissions;

namespace Snow.AuthorityManagement.Application.Authorization.Roles
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public class RoleAppService : IRoleAppService
    {
        private readonly IMapper _mapper;
        private readonly DbContext CurrentContext;
        private readonly IPermissionStore _permissionStore;
        private readonly ILambdaRepository<Role> _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="context"></param>
        /// <param name="roleRepository"></param>
        /// <param name="permissionRepository"></param>
        public RoleAppService(
            IMapper mapper
            , IPermissionStore permissionStore
            , AuthorityManagementContext context
            , ILambdaRepository<Role> roleRepository
            , IPermissionRepository permissionRepository
            , IPermissionDefinitionManager permissionDefinitionManager)
        {
            _mapper = mapper;
            CurrentContext = context;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _permissionStore = permissionStore;
            _permissionDefinitionManager = permissionDefinitionManager;
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleListDto>> GetAllRoleListAsync()
        {
            List<Role> roles = await _roleRepository.GetAllListAsync();
            return _mapper.Map<List<RoleListDto>>(roles);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        public async Task<PagedResultDto<RoleListDto>> GetPagedAsync(GetRolesInput input)
        {
            List<string> wheres = new List<string>();
            List<object> parameters = new List<object>();
            int index = 0;
            if (!string.IsNullOrEmpty(input.DisplayName))
            {
                wheres.Add($"DisplayName.Contains(@{index++})");
                parameters.Add(input.DisplayName);
            }
            if (!string.IsNullOrEmpty(input.Date))
            {
                DateTime[] date = Array.ConvertAll(input.Date
                        .Split(new[] { '~' }, StringSplitOptions.RemoveEmptyEntries)
                    , DateTime.Parse);
                wheres.Add($"AddTime > (@{index++}) AND AddTime < (@{index++})");
                parameters.Add(date[0]);
                parameters.Add(date[1]);
            }
            if (!string.IsNullOrEmpty(input.Sorting))
            {
                input.Sorting = input.Sorting + (input.Order == OrderType.ASC ? " ASC" : " DESC");
            }
            var result = await _roleRepository
                .GetPagedAsync(input.PageIndex, input.PageSize,
                    string.Join(" AND ", wheres), parameters.ToArray(), input.Sorting);
            return new PagedResultDto<RoleListDto>()
            {
                Items = _mapper.Map<List<RoleListDto>>(result.Item1),
                TotalCount = result.Item2,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            };
        }

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public async Task<RoleEditDto> GetForEditAsync(int roleId)
        {
            Role role = await _roleRepository.GetAsync(roleId);
            RoleEditDto roleEditDto = _mapper.Map<RoleEditDto>(role);
            return roleEditDto;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public async Task<string> GetPermissionsAsync(string roleName)
        {
            IEnumerable<AncPermission> permissions = null;
            if (roleName != null)
            {
                permissions = await _permissionStore.GetPermissionsByRoleNamesAsync(roleName);
            }
            IReadOnlyList<PermissionDefinition> permissionDefinitions = _permissionDefinitionManager.GetPermissions();
            ICollection<FlatPermissionDto> result = new List<FlatPermissionDto>();
            foreach (PermissionDefinition permission in permissionDefinitions.Where(p => p.Parent == null))
            {
                result.Add(AddPermission(permission, permissions));
            }
            return Serialization.SerializeObjectCamel(result);
        }

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public async Task<GetRoleForEditOutput> GetForEditAsync(int? roleId)
        {
            List<int> l = new List<int>();
            RoleEditDto roleEditDto = null;
            IEnumerable<AncPermission> permissions = null;
            if (roleId.HasValue)
            {
                Role role = await _roleRepository.GetAsync(roleId.Value);
                roleEditDto = _mapper.Map<RoleEditDto>(role);
                permissions = await _permissionStore.GetPermissionsByRoleNamesAsync(role.Name);
            }

            IReadOnlyList<PermissionDefinition> permissionDefinitions = _permissionDefinitionManager.GetPermissions();
            ICollection<FlatPermissionDto> result = new List<FlatPermissionDto>();
            foreach (PermissionDefinition permission in permissionDefinitions.Where(p => p.Parent == null))
            {
                result.Add(AddPermission(permission, permissions));
            }
            return new GetRoleForEditOutput()
            {
                Role = roleEditDto,
                Permission = Serialization.SerializeObjectCamel(result)
            };
        }

        private FlatPermissionDto AddPermission(PermissionDefinition permission
           , IEnumerable<AncPermission> oldPermissions)
        {
            var flatPermission = _mapper.Map<FlatPermissionDto>(permission);
            if (oldPermissions != null && oldPermissions.Any(op => op.Name == permission.Name))
            {
                flatPermission.IsGranted = true;
            }
            foreach (PermissionDefinition child in permission.Children)
            {
                if (flatPermission.Children == null)
                {
                    flatPermission.Children = new List<FlatPermissionDto>();
                }

                flatPermission.Children.Add(AddPermission(child, oldPermissions));
            }

            return flatPermission;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input">信息</param>
        /// <param name="permissionNames">权限</param>
        /// <returns>信息</returns>
        [UnitOfWork]
        public virtual async Task<RoleListDto> CreateAsync(RoleEditDto input, List<string> permissionNames)
        {
            if (await _roleRepository.ExistsAsync(u => u.Name == input.Name))
            {
                throw new UserFriendlyException("角色名已存在");
            }
            Role role = _mapper.Map<Role>(input);
            role.Id = await _roleRepository.InsertAndGetIdAsync(role);
            role.LastModificationTime = role.CreationTime;

            #region 权限

            List<AncPermission> permissions = CreatePermissions(role.Name, permissionNames);
            await _permissionRepository.InsertAsync(permissions);

            #endregion 权限

            return _mapper.Map<RoleListDto>(role);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input">信息</param>
        /// <param name="permissionNames">权限</param>
        /// <returns>信息</returns>
        [UnitOfWork]
        public virtual async Task<RoleListDto> EditAsync(RoleEditDto input, List<string> permissionNames)
        {
            #region 角色

            Role oldRole = await _roleRepository.GetAsync(input.ID.Value)
                ?? throw new UserFriendlyException("角色不存在");
            Role role = _mapper.Map(input, oldRole);
            role.LastModificationTime = DateTime.Now;
            await _roleRepository.UpdateAsync(role);

            #endregion 角色

            #region 权限

            List<AncPermission> permissions = CreatePermissions(role.Name, permissionNames);
            await _permissionRepository.SetPermissionsByRoleNameAsync(role.Name, permissions);

            #endregion 权限

            return _mapper.Map<RoleListDto>(role);
        }

        /// <summary>
        /// 构建权限实体
        /// </summary>
        /// <param name="providerKey">提供者标识</param>
        /// <param name="permissionNames">权限名集合</param>
        /// <returns>数据库实体集合</returns>
        private List<AncPermission> CreatePermissions(string providerKey, IEnumerable<string> permissionNames)
        {
            if (permissionNames == null)
            {
                throw new ArgumentNullException(nameof(permissionNames));
            }

            List<AncPermission> permissionsEntities = new List<AncPermission>();
            DateTime now = DateTime.Now;
            permissionsEntities.AddRange(permissionNames.Select(p => new AncPermission
            {
                ProviderName = AncConsts.PermissionRoleProviderName,
                ProviderKey = providerKey,
                CreationTime = now,
                Name = p
            }));

            return permissionsEntities;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<bool> DeleteAsync(int id)
        {
            Role role = await _roleRepository.GetAsync(id)
                        ?? throw new UserFriendlyException("角色不存在");

            List<AncPermission> permissions = await _permissionRepository.GetPermissionsByRoleNameAsync(role.Name);
            await _permissionRepository.DeleteAsync(permissions);
            await _roleRepository.DeleteAsync(role);
            return true;
        }
    }
}
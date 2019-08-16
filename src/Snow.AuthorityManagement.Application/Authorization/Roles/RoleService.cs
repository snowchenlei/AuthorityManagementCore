﻿using AutoMapper;
using Snow.AuthorityManagement.Core.Entities;
using Snow.AuthorityManagement.Core.Enum;
using Snow.AuthorityManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Core.Model;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Authorization.Permissions;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Dto;
using Snow.AuthorityManagement.Common.Conversion;
using Snow.AuthorityManagement.Application.Authorization.Permissions.Dto;
using Snow.AuthorityManagement.Common.Authorization;
using System.Collections.Immutable;

namespace Snow.AuthorityManagement.Application.Authorization.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly DbContext CurrentContext;
        private readonly PermissionDefinitionContextBase _context;
        private readonly IBaseRepository<Role> CurrentRepository;
        private readonly IPermissionRepository _permissionRepository;

        public RoleService(
            IMapper mapper
            , AuthorityManagementContext context
            , IBaseRepository<Role> currentRepository, IPermissionRepository permissionRepository)
        {
            _mapper = mapper;
            CurrentContext = context;
            _context = PermissionDefinitionContextBase.Context;
            CurrentRepository = currentRepository;
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input">过滤条件</param>
        /// <returns></returns>
        public async Task<PagedResultDto<RoleListDto>> GetPagedAsync(GetRoleInput input)
        {
            List<string> wheres = new List<string>();
            List<object> parameters = new List<object>();
            int index = 0;
            if (!string.IsNullOrEmpty(input.Name))
            {
                wheres.Add($"Name.Contains(@{index++})");
                parameters.Add(input.Name);
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
            var result = await CurrentRepository
                .GetPagedAsync(input.PageIndex, input.PageSize,
                    string.Join(" AND ", wheres), parameters.ToArray(), input.Sorting);
            return new PagedResultDto<RoleListDto>()
            {
                Items = _mapper.Map<List<RoleListDto>>(result.Item1),
                TotalCount = result.Item2
            };
        }

        /// <summary>
        /// 根据Id获取数据
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public async Task<GetRoleForEditOutput> GetForEditAsync(int? roleId)
        {
            RoleEditDto roleEditDto = null;
            if (roleId.HasValue)
            {
                Role role = await CurrentRepository.FirstOrDefaultAsync(u => u.ID == roleId);
                roleEditDto = _mapper.Map<RoleEditDto>(role);
            }

            IReadOnlyList<AncPermission> permissions = _context
                .Permissions.Values.ToImmutableList();
            ICollection<FlatPermissionDto> result = new List<FlatPermissionDto>();
            foreach (AncPermission permission in permissions.Where(p => p.Parent == null))
            {
                result.Add(AddPermission(permission, null));
            }
            return new GetRoleForEditOutput()
            {
                Role = roleEditDto,
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

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input">信息</param>
        /// <param name="permission">权限</param>
        /// <returns>信息</returns>
        public async Task<RoleListDto> AddAsync(RoleEditDto input, string permission)
        {
            if (await CurrentRepository.IsExistsAsync(u => u.Name == input.Name))
            {
                throw new UserFriendlyException("角色名已存在");
            }
            Role role = _mapper.Map<Role>(input);
            role.Permissions = CreatePermissions(permission);
            role = await CurrentRepository.AddAsync(role);

            await CurrentContext.SaveChangesAsync();
            return _mapper.Map<RoleListDto>(role);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input">信息</param>
        /// <param name="permission">权限</param>
        /// <returns>信息</returns>
        public async Task<RoleListDto> EditAsync(RoleEditDto input, string permission)
        {
            #region 角色

            Role oldRole = await CurrentRepository
                    .FirstOrDefaultAsync(u => u.ID == input.ID.Value);
            if (oldRole == null)
            {
                throw new UserFriendlyException("角色不存在");
            }
            Role role = _mapper.Map(input, oldRole);
            CurrentRepository.Edit(role);

            #endregion 角色

            #region 权限

            List<Permission> permissions = CreatePermissions(permission);
            List<Permission> oldPermissions = await _permissionRepository
                .LoadListAsync(p => p.Role.ID == role.ID);
            List<Permission> newPermissions = permissions
                .Except(oldPermissions, new PermissionComparer()).ToList();
            List<Permission> lostPermissions = oldPermissions
                .Except(permissions, new PermissionComparer()).ToList();
            await _permissionRepository.SetPermissionsByRoleId(role, newPermissions,
                lostPermissions);

            #endregion 权限

            if (await CurrentContext.SaveChangesAsync() <= 0)
            {
                throw new UserFriendlyException("修改失败");
            }

            return _mapper.Map<RoleListDto>(role);
        }

        /// <summary>
        /// 构建权限实体
        /// </summary>
        /// <param name="permission">权限字符串</param>
        /// <returns></returns>
        private List<Permission> CreatePermissions(string permission)
        {
            List<Permission> permissionsEntiy = new List<Permission>();
            if (string.IsNullOrEmpty(permission))
            {
                return permissionsEntiy;
            }
            string[] permissions = permission
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Distinct().ToArray();
            DateTime now = DateTime.Now;
            foreach (string per in permissions)
            {
                permissionsEntiy.Add(new Permission()
                {
                    CreationTime = now,
                    IsGranted = true,
                    Name = per
                });
            }

            return permissionsEntiy;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id)
        {
            Role role = await CurrentRepository.FirstOrDefaultAsync(a => a.ID == id)
                        ?? throw new UserFriendlyException("角色不存在");

            CurrentRepository.Delete(role);
            return await CurrentContext.SaveChangesAsync() > 0;
        }
    }
}
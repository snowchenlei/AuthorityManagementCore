using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.IRepository;
using Snow.AuthorityManagement.IService.Authorization;

namespace Snow.AuthorityManagement.Service.Authorization
{
    public class PermissionService : IPermissionService
    {
        private readonly IBaseRepository<Permission> _permissionRepository;

        public PermissionService(IBaseRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<Permission>> GetAllPermissionsAsync(int userId)
        {
            return await _permissionRepository.LoadListAsync(p => p.User.ID == userId);
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            return await _permissionRepository.LoadListAsync(p => p.Role.ID == roleId);
        }
    }
}
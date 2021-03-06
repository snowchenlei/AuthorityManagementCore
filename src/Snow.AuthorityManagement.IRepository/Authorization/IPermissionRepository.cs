﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snow.AuthorityManagement.Core.Entities.Authorization;

namespace Snow.AuthorityManagement.IRepository.Authorization
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<bool> SetPermissionsByRoleId(Role role, List<Permission> newPermissions
            , List<Permission> lostPermissions);
    }
}
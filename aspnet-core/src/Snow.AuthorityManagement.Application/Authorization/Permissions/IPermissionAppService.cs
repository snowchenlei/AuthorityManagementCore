using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Application.Services;
using Anc.Domain.Entities;
using Snow.AuthorityManagement.Application.Authorization.Permissions.Dto;
using Snow.AuthorityManagement.Core.Authorization.Permissions;

namespace Snow.AuthorityManagement.Application.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        Task<IEnumerable<string>> GetUserPermissionsAsync(string[] roleNames);
    }
}
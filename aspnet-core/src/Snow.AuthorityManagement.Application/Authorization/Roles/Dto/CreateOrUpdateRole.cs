using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Roles.Dto
{
    public class CreateOrUpdateRole
    {
        [Display(Name = "角色")]
        public RoleEditDto Role { get; set; }

        public List<string> PermissionNames { get; set; }
    }
}
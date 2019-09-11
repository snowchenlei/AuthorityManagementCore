using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Roles.Dto
{
    public class RoleEditDto
    {
        public int? ID { get; set; }

        [Display(Name = "角色名")]
        public string Name { get; set; }

        [Display(Name = "显示名")]
        public string DisplayName { get; set; }

        [Display(Name = "排序")]
        public int Sort { get; set; }
    }
}
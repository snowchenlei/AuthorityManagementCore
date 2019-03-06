using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Snow.AuthorityManagement.Core.Dto.Role
{
    public class RoleEditDto
    {
        public int? ID { get; set; }

        [Display(Name = "角色名")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "排序")]
        public int Sort { get; set; }
    }
}
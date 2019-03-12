using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Snow.AuthorityManagement.Core.Dto.Role
{
    public class CreateOrUpdateRole
    {
        [Required]
        public RoleEditDto Role { get; set; }

        public string Permission { get; set; }
    }
}
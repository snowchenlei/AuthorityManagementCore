using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Dto.Role
{
    public class RoleListDto : EntityDto
    {
        public string Name { get; set; }
        public int Sort { get; set; }
    }
}
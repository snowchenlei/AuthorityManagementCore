using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Dto.Role
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }
        public string Permission { get; set; }
    }
}
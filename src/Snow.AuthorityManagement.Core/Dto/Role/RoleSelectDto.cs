using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Dto.Role
{
    public class RoleSelectDto
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
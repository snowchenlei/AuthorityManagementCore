using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Dto.Role
{
    public class GetRoleInput : PagedAndSortedInputDto
    {
        public string Name { get; set; }
        public string Date { get; set; }
    }
}
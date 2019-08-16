using Snow.AuthorityManagement.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Roles.Dto
{
    public class GetRoleInput : PagedAndSortedInputDto
    {
        public string Name { get; set; }
        public string Date { get; set; }
    }
}
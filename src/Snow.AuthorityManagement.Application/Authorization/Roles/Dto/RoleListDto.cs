using Snow.AuthorityManagement.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Roles.Dto
{
    public class RoleListDto : EntityDto
    {
        public string Name { get; set; }
        public int Sort { get; set; }
    }
}
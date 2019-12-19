using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Models.Roles
{
    public class CreateOrEditRoleModalViewModel
    {
        public CreateOrEditRoleModalViewModel()
        {
            //Mapper.Map<CreateOrEditRoleModalViewModel>(output);
        }

        public RoleEditDto Role { get; set; }
        public string Permission { get; set; }
    }
}
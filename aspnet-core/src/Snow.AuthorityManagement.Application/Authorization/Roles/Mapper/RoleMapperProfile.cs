using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Core.Authorization.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Roles.Mapper
{
    public class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            CreateMap<Role, RoleListDto>();
            CreateMap<RoleEditDto, Role>();
            CreateMap<Role, RoleEditDto>();
        }
    }
}
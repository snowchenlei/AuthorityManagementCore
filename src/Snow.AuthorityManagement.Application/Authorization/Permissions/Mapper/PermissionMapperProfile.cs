using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Permissions.Dto;
using Snow.AuthorityManagement.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Permissions.Mapper
{
    public class PermissionMapperProfile : Profile
    {
        public PermissionMapperProfile()
        {
            CreateMap<AncPermission, FlatPermissionDto>()
               .ForMember(p => p.Children, opt => opt.MapFrom(src => src.Children.Count > 0 ? src.Children : null))
               .ForMember(p => p.Children, opt => opt.Ignore());
        }
    }
}
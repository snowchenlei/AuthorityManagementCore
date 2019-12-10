﻿using Anc.Authorization;
using Anc.Authorization.Permissions;
using Anc.Domain.Entities;
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
            CreateMap<PermissionDefinition, FlatPermissionDto>()
                .ForMember(entity=>entity.Children, opt=>opt.Ignore())
                .ForMember(entity => entity.ParentName,
                    opt => opt.MapFrom(src => (src.Parent.Name)));
        }
    }
}
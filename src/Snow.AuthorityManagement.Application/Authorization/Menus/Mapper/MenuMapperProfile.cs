using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Core.Authorization.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Menus.Mapper
{
    public class MenuMapperProfile : Profile
    {
        public MenuMapperProfile()
        {
            CreateMap<Menu, MenuListDto>();
            CreateMap<MenuEditDto, Menu>();
            CreateMap<Menu, MenuEditDto>()
                .ForMember(a => a.ParentID,
                    opt => opt.MapFrom(src => src.Parent == null ? null : (int?)src.Parent.ID));
        }
    }
}
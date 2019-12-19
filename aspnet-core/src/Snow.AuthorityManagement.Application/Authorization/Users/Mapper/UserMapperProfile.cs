using AutoMapper;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Application.Authorization.Users.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserLoginOutput>();
            CreateMap<User, UserListDto>();
            CreateMap<UserEditDto, User>();
            CreateMap<User, UserEditDto>();
        }
    }
}
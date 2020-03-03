using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Snow.AuthorityManagement.Core.Authorization.Logs;
using Snow.AuthorityManagement.Application.Authorization.Logs.Dto;

namespace Snow.AuthorityManagement.Application.Authorization.Logs.Mapper
{
    public class LogMapperProfile : Profile
    {
        public LogMapperProfile()
        {
            CreateMap<Log, LogListDto>();
        }
    }
}
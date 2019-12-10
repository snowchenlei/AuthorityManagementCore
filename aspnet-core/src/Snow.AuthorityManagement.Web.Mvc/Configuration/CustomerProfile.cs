﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Web.Models.Roles;
using Snow.AuthorityManagement.Web.Mvc.Models.Menus;
using Snow.AuthorityManagement.Web.Mvc.Models.Users;

namespace Snow.AuthorityManagement.Web.Configuration
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<GetUserForEditOutput, CreateOrEditUserModalViewModel>();
            CreateMap<GetRoleForEditOutput, CreateOrEditRoleModalViewModel>();
            CreateMap<MenuEditDto, CreateOrEditMenuModalViewModel>();
            CreateMap<MenuListDto, SelectListItem>()
                .ForMember(a => a.Value, opt => opt.MapFrom(src => src.ID))
                .ForMember(a => a.Text, opt => opt.MapFrom(src => src.Name));
            CreateMap<RoleListDto, SelectListItem>()
                .ForMember(a => a.Value, opt => opt.MapFrom(src => src.ID))
                .ForMember(a => a.Text, opt => opt.MapFrom(src => src.DisplayName));

            //Mapper.Initialize(x =>
            //{
            //    x.CreateMap<UserEdit, User>();
            //    x.CreateMap<RoleEdit, Role>();
            //    x.CreateMap<PermissionEdit, Permission>();
            //    x.CreateMap<ModuleEdit, Entity.Module>();
            //    x.CreateMap<ModuleElementEdit, ModuleElement>();
            //});

            //IAreaServices areaServices = container.Resolve<IAreaServices>();
            //Mapper.Initialize(x =>
            //{
            //    x.CreateMap<MerchantRegister, Merchant>()
            //        .ForMember(entity => entity.City,
            //            opt => opt.MapFrom(src => areaServices
            //                .LoadEntities(a => a.ID == src.CityId).FirstOrDefault()))
            //        .ForMember(entity => entity.Province,
            //        opt => opt.MapFrom(src => areaServices
            //        .LoadEntities(a => a.ID == src.ProvinceId).FirstOrDefault()));
            //});

            /*IAgentServices agentServices = container.Resolve<IAgentServices>();
            IBankHeadOfficeServices bankHeadOfficeServices = container.Resolve<IBankHeadOfficeServices>();
            IBankBranchServices bankBranchServices = container.Resolve<IBankBranchServices>();
            Mapper.Initialize(x =>
            {
                x.CreateMap<MerchantRegister, Merchant>();
                x.CreateMap<BankCardBind, BankCard>();
                x.CreateMap<RepaymentPlanAdd, MerchantPlan>();
                x.CreateMap<ManagerRegister, User>();
                x.CreateMap<ManagerAgent, Agent>();
                    //.ForMember(entity => entity.Parent,
                    //    opt => opt.MapFrom(src => agentServices
                    //        .LoadFirst(obj => obj.ID == src.ParentId)))
                    //.ForMember(entity => entity.BankHeadOffice,
                    //    opt => opt.MapFrom(src => bankHeadOfficeServices
                    //        .LoadFirst(obj => obj.ID == src.BankId)))
                    //.ForMember(entity => entity.BankBranch,
                    //    opt => opt.MapFrom(src => bankBranchServices
                    //        .LoadFirst(obj => obj.ID == src.BankBranchId)));
                x.CreateMap<ModuleView, Entity.Module>();
                x.CreateMap<ModuleElementView, ModuleElement>();
                x.CreateMap<TransRecordModel, TransRecord>();
                x.CreateMap<TransRecord, TransRecord>();
            });*/
        }
    }
}
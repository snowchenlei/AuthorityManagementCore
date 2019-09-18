using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Validators;
using Snow.AuthorityManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Web.Core.Startup
{
    public static class AuthorityManagementServiceCollectionExtensions
    {
        public static void AddAnc(this IServiceCollection services)
        {
        }

        public static void AddFluentValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<UserEditDto>, UserEditValidator>();
            services.AddTransient<IValidator<CreateOrUpdateUser>, CreateOrUpdateUserValidator>();
        }
    }
}
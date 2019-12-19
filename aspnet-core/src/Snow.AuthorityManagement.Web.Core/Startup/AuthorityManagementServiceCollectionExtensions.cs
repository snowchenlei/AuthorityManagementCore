using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Application.Authorization.Menus.Validators;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Validators;

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
            services.AddTransient<IValidator<MenuEditDto>, MenuEditValidator>();
            services.AddTransient<IValidator<CreateOrUpdateUser>, CreateOrUpdateUserValidator>();
        }
    }
}
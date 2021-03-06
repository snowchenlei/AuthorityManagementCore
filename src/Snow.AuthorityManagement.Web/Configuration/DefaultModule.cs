﻿using Autofac;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Snow.AuthorityManagement.Core.Entities.Authorization;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.IRepository;
using Snow.AuthorityManagement.IService.Authorization;
using Snow.AuthorityManagement.Repository;
using Snow.AuthorityManagement.Service.Authorization;
using Snow.AuthorityManagement.Web.Authorization;
using Snow.AuthorityManagement.Web.Manager;
using Snow.AuthorityManagement.Web.Session;

namespace Snow.AuthorityManagement.Web.Configuration
{
    public class DefaultModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var iService = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.IService"));
            var service = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Service"));
            var iRepository = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.IRepository"));
            var repository = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Repository"));

            builder.RegisterAssemblyTypes(iService, service)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(iRepository, repository)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>));
            builder.RegisterType<AuthorizationHelper>().As<IAuthorizationHelper>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            builder.RegisterType<UserNavigationManager>().As<IUserNavigationManager>();
            builder.RegisterType<ClaimsAncSession>().As<IAncSession>();
        }
    }
}
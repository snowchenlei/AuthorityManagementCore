using Autofac;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Http;
using Snow.AuthorityManagement.Web.Authorization;
using Snow.AuthorityManagement.Web.Manager;
using Snow.AuthorityManagement.Web.Session;
using Anc.Dependency;
using Anc.Domain.Repositories;
using Anc.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.Repository;
using Anc.Domain.Uow;
using Anc.EntityFrameworkCore.Uow;
using Snow.AuthorityManagement.Data.Dapper.Repositories;
using Autofac.Extras.DynamicProxy;
using Anc.Runtime.Session;
using Anc.Authorization;
using Snow.AuthorityManagement.Repository.Authorization.Permissions.DomainService;
using Anc.Application.Navigation;

namespace Snow.AuthorityManagement.Web.Configuration
{
    public class DefaultModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var application = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Application"));
            var mvc = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Web.Mvc"));
            var anc = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc"));
            var ancAspCore = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc.AspNetCore"));
            var ancEfCore = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc.EntityFrameworkCore"));
            var data = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Data"));
            var rep = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Repository"));
            var core = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Core"));

            //var service = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Service"));
            //var iRepository = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.IRepository"));
            //var repository = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Repository"));

            //builder.RegisterAssemblyTypes(iService, service)
            //    .Where(t => t.Name.EndsWith("Service"))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();
            //builder.RegisterAssemblyTypes(iRepository, repository)
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWorkInterceptor>();

            var transientType = typeof(ITransientDependency);
            var singletonType = typeof(ISingletonDependency);
            builder.RegisterAssemblyTypes(application, anc, ancAspCore, ancEfCore, data, rep, core, mvc)
                .Where(m => transientType.IsAssignableFrom(m) && m != transientType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .InterceptedBy(typeof(UnitOfWorkInterceptor)).EnableClassInterceptors();
            builder.RegisterAssemblyTypes(application, anc, ancAspCore, ancEfCore, data, rep, core)
               .Where(m => singletonType.IsAssignableFrom(m) && m != singletonType)
               .AsImplementedInterfaces()
               .SingleInstance()
               .InterceptedBy(typeof(UnitOfWorkInterceptor)).EnableClassInterceptors();
            //builder.RegisterAssemblyTypes(rep, core)
            //    .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();
            //builder.RegisterAssemblyTypes(anc)
            //    .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
            //    .AsImplementedInterfaces().InstancePerLifetimeScope();
            //builder.RegisterAssemblyTypes(data)
            //   .Where(m => baseType.IsAssignableFrom(m) && m != baseType)
            //   .AsImplementedInterfaces().InstancePerLifetimeScope();

            //builder.RegisterType<UserService>().As<IUserService>();
            //builder.RegisterType<RoleService>().As<IRoleService>();
            //builder.RegisterType<PermissionService>().As<IPermissionService>();
            //builder.RegisterType<PermissionRepository>().As<IPermissionRepository>();

            builder.RegisterGeneric(typeof(AuthorityManagementRepositoryBase<>)).As(typeof(ILambdaRepository<>));
            builder.RegisterGeneric(typeof(AuthorityManagementRepositoryBase<,>)).As(typeof(ILambdaRepository<,>));
            builder.RegisterGeneric(typeof(AuthorityManagementRepositoryBase<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(AuthorityManagementRepositoryBase<,>)).As(typeof(IRepository<,>));
            builder.RegisterType<EfCoreUnitOfWork<AuthorityManagementContext>>().As<IUnitOfWork>();
            builder.RegisterType<UserNavigationManager>().As<IUserNavigationManager>();
            //builder.RegisterType<PermissionRepository>().As<IPermissionRepository>();
            //builder.RegisterType<AuthorizationHelper>().As<IAuthorizationHelper>();
            //builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            //builder.RegisterType<ClaimsAncSession>().As<IAncSession>();
            //builder.RegisterType<PermissionManager>().As<IPermissionManagerBase>();
        }
    }
}
using Autofac;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Anc.Domain.Repositories;
using Anc.Domain.Uow;
using Anc.EntityFrameworkCore.Uow;
using Anc.Application.Navigation;
using Anc.DependencyInjection;
using Snow.AuthorityManagement.Repositories;
using Snow.AuthorityManagement.EntityFrameworkCore;
using Autofac.Extras.DynamicProxy;

namespace Snow.AuthorityManagement.Web.Configuration
{
    public class DefaultModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // TODO:自动依赖注入

            var application = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Application"));
            var mvc = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Web.Mvc"));
            var entityFrameworkCore = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.EntityFrameworkCore"));
            var ancAspCore = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc.AspNetCore"));
            var ancAspCoreMvc = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc.AspNetCore.Mvc"));
            var ancCore = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc.Core"));
            var ancThreading = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc.Threading"));
            var ancSecurit = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc.Security"));
            var ancAuthorization = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc.Authorization"));
            var ancEfCore = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Anc.EntityFrameworkCore"));
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
            Assembly[] assemblies = new Assembly[] { application, ancAspCoreMvc, entityFrameworkCore, ancAuthorization, ancCore, ancThreading, ancSecurit, ancAspCore, ancEfCore, rep, core, mvc };
            var transientType = typeof(ITransientDependency);
            var singletonType = typeof(ISingletonDependency);
            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(m => transientType.IsAssignableFrom(m) && m != transientType)
            //    .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(m => transientType.IsAssignableFrom(m) && m != transientType)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
            .InterceptedBy(typeof(UnitOfWorkInterceptor)).EnableClassInterceptors();
            //builder.RegisterAssemblyTypes(assemblies)
            //    .Where(m => singletonType.IsAssignableFrom(m) && m != singletonType)
            //    .SingleInstance();
            builder.RegisterAssemblyTypes(assemblies)
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
            builder.RegisterType<EfCoreUnitOfWork<AuthorityManagementContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<UserNavigationManager>().As<IUserNavigationManager>();
            //builder.RegisterType<PermissionDefinitionContextBase>().As<IPermissionDefinitionContext>();
            //builder.RegisterType<PermissionRepository>().As<IPermissionRepository>();
            //builder.RegisterType<AuthorizationHelper>().As<IAuthorizationHelper>();
            //builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            //builder.RegisterType<ClaimsAncSession>().As<IAncSession>();
            //builder.RegisterType<PermissionManager>().As<IPermissionManagerBase>();
        }
    }
}
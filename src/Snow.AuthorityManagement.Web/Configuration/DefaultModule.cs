using Autofac;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Castle.Core.Logging;

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
        }
    }
}
using Autofac;
using System.Reflection;
using System.Runtime.Loader;

namespace Cl.AuthorityManagement.Api
{
    public class DefaultModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var iServices = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Cl.AuthorityManagement.IServices"));
            var services = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Cl.AuthorityManagement.Services"));
            var iRepository = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Cl.AuthorityManagement.IRepository"));
            var repository = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Cl.AuthorityManagement.Repository"));

            builder.RegisterAssemblyTypes(iServices, services)
                .Where(t => t.Name.EndsWith("Services"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(iRepository, repository)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
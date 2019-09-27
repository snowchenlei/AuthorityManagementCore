using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Snow.AuthorityManagement.Web.Startup.OnceTask;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Snow.AuthorityManagement.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.RunWithTasksAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddJsonFile("cache.json", true);
                 })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup.Startup>();
                 });
    }
}
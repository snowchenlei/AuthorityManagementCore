using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Snow.AuthorityManagement.Web.Startup.OnceTask;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using Microsoft.Extensions.Logging;

namespace Snow.AuthorityManagement.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var host = CreateHostBuilder(args).Build();

            await host.RunWithTasksAsync();
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid
            // segmentation fault on Linux)
            NLog.LogManager.Shutdown();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddJsonFile("cache.json", true);
                     config.AddJsonFile("hosting.json", true);
                 })
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder
                         .ConfigureLogging(logging =>
                         {
                             logging.ClearProviders();
                             logging.SetMinimumLevel(LogLevel.Trace);
                         })
                         .UseNLog()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup.Startup>();
                 });
    }
}
﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Snow.AuthorityManagement.Web.Startup.OnceTask;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog;
using System;

namespace Snow.AuthorityManagement.Web
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
           .AddEnvironmentVariables()
           .Build();

        private static string LogFilePath = Path.Combine("App_Data", "Logs", DateTime.Now.ToString("yyyyMM"));

        public static async Task<int> Main(string[] args)
        {
            // Serilog配置 https://github.com/serilog/serilog/wiki/Configuration-Basics
            Log.Logger = new LoggerConfiguration()
                // 最低日志级别
                .MinimumLevel.Information()
                // 重写微软最低日志级别
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // TODO:过滤微软日志存单独文件
                .WriteTo.Logger(lg => lg
                    // 过滤级别
                    .Filter.ByIncludingOnly(p => p.Level.Equals(LogEventLevel.Information))
                    // 记录日志
                    .WriteTo.File(Path.Combine(LogFilePath, "info.log"), LogEventLevel.Information
                        , rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true))
                // 到指定大小才写入日志
                //.WriteTo.Async(a => a.File(Path.Combine(LogFilePath, "info.log"), LogEventLevel.Information
                //    , rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)), 500)
                .WriteTo.Logger(lg => lg
                    .Filter.ByIncludingOnly(p => p.Level.Equals(LogEventLevel.Error))
                    .WriteTo.Async(a => a.File(Path.Combine(LogFilePath, "error.log"), LogEventLevel.Error
                        , rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)))
                .CreateLogger();
            try
            {
                Log.Information("Starting web host1");
                Log.Information("Starting web host2");
                Log.Information("Starting web host3");
                Log.Error("error web host");
                var host = CreateHostBuilder(args).Build();

                await host.RunWithTasksAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
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
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup.Startup>();
                 })
            .UseSerilog();
    }
}
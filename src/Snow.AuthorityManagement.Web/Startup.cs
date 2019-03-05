using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Snow.AuthorityManagement.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Snow.AuthorityManagement.Web.Configuration;
using Snow.AuthorityManagement.Web.Library;
using Microsoft.Extensions.Logging;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Web.Library.Middleware;

namespace Snow.AuthorityManagement.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private ILogger<Startup> _logger;

        public Startup(IConfiguration configuration
            , ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region 线程内唯一

            //IServiceCollection services = new ServiceCollection();
            services.AddEntityFrameworkSqlServer().AddDbContext<AuthorityManagementContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Scoped);

            #endregion 线程内唯一

            services.AddAutoMapper();

            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(CustomerExceptionAttribute));
                //options.Filters.Add(typeof(CustomerResultAttribute));

                #region 输出缓存配置

                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 120
                    });
                options.CacheProfiles.Add("Header",
                    new CacheProfile()
                    {
                        Duration = 120,
                        VaryByHeader = "User-Agent"
                    });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });

                #endregion 输出缓存配置
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // 设置超时时间
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            #region Autofac

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<DefaultModule>();
            builder.Populate(services);
            IContainer container = builder.Build();
            return new AutofacServiceProvider(container);

            #endregion Autofac
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
                //app.UseExceptionHandler(build =>
                //    build.Run(async context =>
                //    {
                //var ex = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
                //if (ex != null)
                //{
                //    string innerException = String.Empty;
                //    while (ex.InnerException != null)
                //    {
                //        ex = ex.InnerException;
                //        innerException += ex.InnerException?.Message + "\r\n" + ex.InnerException?.StackTrace + "\r\n";
                //    }
                //    string message = $@"【{ex.Message}】内部错误【{ex.InnerException?.Message}】";
                //    _logger.LogError(ex, message);
                //    if (ex is UserFriendlyException)
                //    {
                //        context.Response.StatusCode = 400;
                //        context.Response.ContentType = "text/plain;charset=utf-8";
                //        await context.Response.WriteAsync(ex.Message);
                //    }
                //    else
                //    {
                //        context.Response.StatusCode = 500;
                //        context.Response.ContentType = "text/plain;charset=utf-8";
                //        await context.Response.WriteAsync("服务器正忙，请稍后重试");
                //    }
                //}
                //else
                //{
                //    context.Response.StatusCode = 500;
                //    if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                //    {
                //        context.Response.ContentType = "text/html";
                //        await context.Response.SendFileAsync($@"{env.WebRootPath}/errors/500.html");
                //    }
                //}
                //}));
                app.UseHsts();
            }

            app.UseCustomerExceptionHandler();
            //使用静态文件
            app.UseStaticFiles();
            //Session
            app.UseSession();
            //app.UseCookiePolicy();//添加后会导致Session失效
            app.UseHttpsRedirection();
            loggerFactory.AddLog4Net();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
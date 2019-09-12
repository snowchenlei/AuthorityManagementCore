using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Anc.Application.Services.Dto;
using Anc.AspNetCore.Web.Mvc.Authorization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CacheCow.Server.Core.Mvc;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Authorization.Roles.Validators;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Validators;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.Web.Authorization;
using Snow.AuthorityManagement.Web.Configuration;
using Snow.AuthorityManagement.Web.Core.Common.ETag.User;
using Snow.AuthorityManagement.Web.Library;
using Snow.AuthorityManagement.Web.Library.Middleware;
using Snow.AuthorityManagement.Web.Startup.OnceTask;

namespace Snow.AuthorityManagement.Web.Startup
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
                // This lambda determines whether user consent for non-essential cookies is needed
                // for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region CacheManager缓存

            services.AddLogging(c => c.AddConsole().AddDebug().AddConfiguration(Configuration));
            // using the new overload which adds a singleton of the configuration to services and the configure method to add logging
            //services.AddCacheManagerConfiguration(Configuration, cfg => cfg.WithMicrosoftLogging(services));
            services.AddCacheManagerConfiguration(Configuration);
            // any other type will be this. Configurastion used will be the one defined by
            // AddCacheManagerConfiguration earlier.
            services.AddCacheManager();

            #endregion CacheManager缓存

            services.AddMemoryCache();

            #region 线程内唯一

            //IServiceCollection services = new ServiceCollection();
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<AuthorityManagementContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Scoped);

            #endregion 线程内唯一

            AddAutoMapper(services);
            //禁用 dotnet core 2.1的formbody等模式自动校验和转换
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //services.AddScoped<CustomCookieAuthenticationEvents>();
            AddAuthentication(services);
            AddMvc(services);

            AddFluentValidation(services);

            AddStartupTask(services);
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // 设置超时时间
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
            AddCacheCow(services);

            return AddIoc(services);
        }

        private void AddMvc(IServiceCollection services)
        {
            var mall = Assembly.Load(new AssemblyName("Snow.AuthorityManagement.Web.Core")); //类库的程序集名称

            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(CustomerExceptionAttribute));
                options.Filters.Add(typeof(AncAuthorizeFilter));
                options.Filters.Add(typeof(ModelStateInvalidFilter));// 自定义模型验证(ApiController无需此语句，可自动验证)

                #region 输出缓存配置

                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 120
                    });
                options.CacheProfiles.Add("Header",
                    new CacheProfile()
                    {
#if DEBUG
                        Duration = 0,
#else
                        Duration = 120,
#endif

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
            })
            .AddApplicationPart(mall)
            //启用FluentValidation验证
            .AddFluentValidation(fv =>
            {
                //禁用其它的认证
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// 注入授权
        /// </summary>
        /// <param name="services"></param>
        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
           .AddCookie(options =>
           {
               // 在这里可以根据需要添加一些Cookie认证相关的配置。
               options.AccessDeniedPath = "/Home/Welcome";//未通过授权
               options.LoginPath = "/Account/Login";//未登录时
               options.LogoutPath = "/Account/Logout";//退出
                                                      //options.EventsType = typeof(CustomCookieAuthenticationEvents);//影响性能
           });
        }

        /// <summary>
        /// 注入AutoMapper
        /// </summary>
        /// <param name="services"></param>
        private void AddAutoMapper(IServiceCollection services)
        {
            var application = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Application"));
            var web = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Web.Mvc"));
            AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(web, application);
        }

        /// <summary>
        /// 注册首次执行任务
        /// </summary>
        /// <param name="services"></param>
        private void AddStartupTask(IServiceCollection services)
        {
            services.AddStartupTask<InitalPermissionTask>();
            services.AddStartupTask<InitialHostDbTask>();
        }

        /// <summary>
        /// 注册FluentValidation
        /// </summary>
        /// <param name="services"></param>
        private void AddFluentValidation(IServiceCollection services)
        {
            services.AddTransient<IValidator<UserEditDto>, UserEditValidator>();
            services.AddTransient<IValidator<CreateOrUpdateUser>, CreateOrUpdateUserValidator>();
            services.AddTransient<IValidator<RoleEditDto>, RoleEditValidator>();
            services.AddTransient<IValidator<CreateOrUpdateRole>, CreateOrUpdateRoleValidator>();
        }

        /// <summary>
        /// 注册缓存
        /// </summary>
        /// <param name="services"></param>
        private void AddCacheCow(IServiceCollection services)
        {
            services.AddHttpCachingMvc();
            services.AddQueryProviderAndExtractorForViewModelMvc<GetUserForEditOutput, TimedETagQueryUserRepository, UserETagExtractor>(false);
            services.AddQueryProviderAndExtractorForViewModelMvc<PagedResultDto<UserListDto>, TimedETagQueryUserRepository, UserCollectionETagExtractor>(false);
        }

        /// <summary>
        /// 注册Ioc容器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private IServiceProvider AddIoc(IServiceCollection services)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<DefaultModule>();
            builder.Populate(services);
            IContainer container = builder.Build();
            return new AutofacServiceProvider(container);
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
            app.UseAuthentication();
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
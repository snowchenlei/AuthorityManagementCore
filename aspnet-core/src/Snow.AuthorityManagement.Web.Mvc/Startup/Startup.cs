using System;
using System.Reflection;
using System.Runtime.Loader;
using Anc.Authorization;
using AspNetCoreRateLimit;
using Autofac;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Snow.AuthorityManagement.Application.Authorization.Menus.Dto;
using Snow.AuthorityManagement.Application.Authorization.Menus.Validators;
using Snow.AuthorityManagement.Application.Authorization.Roles.Dto;
using Snow.AuthorityManagement.Application.Authorization.Roles.Validators;
using Snow.AuthorityManagement.Application.Authorization.Users.Dto;
using Snow.AuthorityManagement.Application.Authorization.Users.Validators;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.EntityFrameworkCore;
using Snow.AuthorityManagement.Web.Configuration;
using Snow.AuthorityManagement.Web.Library;
using Snow.AuthorityManagement.Web.Library.Middleware;
using Snow.AuthorityManagement.Web.Startup.OnceTask;
using StackExchange.Profiling.Storage;

namespace Snow.AuthorityManagement.Web.Startup
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed
                // for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // 需要在EF和MiniProfiler之前使用，否则会报错
            services.AddMemoryCache();

            #region 线程内唯一

            //IServiceCollection services = new ServiceCollection();
            services.AddEntityFrameworkMySql()
                .AddDbContext<AuthorityManagementContext>(options =>
            {
                string sqlConnection = Configuration.GetConnectionString(AuthorityManagementConsts.ConnectionStringName);
                options.UseMySql(sqlConnection);
            }, ServiceLifetime.Scoped);

            #endregion 线程内唯一

            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
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

            services.AddHealthChecks()
                .AddDbContextCheck<AuthorityManagementContext>();
            AddRateLimit(services);
#if DEBUG
            AddMiniProfiler(services);
#endif
            AutoAddDefinitionProviders(services);
        }

        private void AddMiniProfiler(IServiceCollection services)
        {
            services.AddMiniProfiler(options =>
            {
            }).AddEntityFramework();
        }

        private void AddRateLimit(IServiceCollection services)
        {
            // needed to load configuration from appsettings.json
            services.AddOptions();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // https://github.com/aspnet/Hosting/issues/793 the IHttpContextAccessor service is not
            // registered by default. the clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        private void AddMvc(IServiceCollection services)
        {
            var mall = Assembly.Load(new AssemblyName("Snow.AuthorityManagement.Web.Core")); //类库的程序集名称

            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(CustomerExceptionAttribute));
                //options.Filters.Add(typeof(AncAuthorizeFilter));
                //options.Filters.Add(typeof(AncAuditActionFilter));
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); // XSRF/CSRF认证
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
            }).AddNewtonsoftJson(options =>
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
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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
            //services.AddStartupTask<InitalPermissionTask>();
            services.AddStartupTask<InitialHostDbTask>();
            // TODO:加载缓存数据
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
            services.AddTransient<IValidator<MenuEditDto>, MenuEditValidator>();
            services.AddTransient<IValidator<CreateOrUpdateMenu>, CreateOrUpdateMenuValidator>();
        }

        private void AutoAddDefinitionProviders(IServiceCollection services)
        {
            //services.OnRegistred(AuthorizationInterceptorRegistrar.RegisterIfNeeded);

            //var definitionProviders = new List<Type>();

            //services.OnRegistred(context =>
            //{
            //    if (typeof(IPermissionDefinitionProvider).IsAssignableFrom(context.ImplementationType))
            //    {
            //        definitionProviders.Add(context.ImplementationType);
            //    }
            //});

            //services.Configure<AncPermissionOptions>(options =>
            //{
            //    options.DefinitionProviders.AddIfNotContains(definitionProviders);
            //});
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DefaultModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/errors");
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
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseIpRateLimiting();
#if DEBUG
            app.UseMiniProfiler();
#endif
            //使用静态文件
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //Session
            app.UseSession();
            app.UseHttpsRedirection();

            app.UseEndpoints(routes =>
            {
                routes.MapHealthChecks("/health");
                routes.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
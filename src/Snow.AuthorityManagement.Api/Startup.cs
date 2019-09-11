using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Anc.AspNetCore.Web.Mvc.Authorization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Snow.AuthorityManagement.Data;
using Snow.AuthorityManagement.Web.Core.Startup;

namespace Cl.AuthorityManagement.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //配置跨域处理
            services.AddCors(options =>
            {
                options.AddPolicy("any", act =>
                {
                    act
                    .WithOrigins("Http://localhost:3000")
                    .AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed
                // for a given request.
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

            var application = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Application"));
            var web = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Snow.AuthorityManagement.Web.Mvc"));
            AppDomain.CurrentDomain.GetAssemblies();
            services.AddAutoMapper(web, application);
            var mall = Assembly.Load(new AssemblyName("Snow.AuthorityManagement.Web.Core")); //类库的程序集名称

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(AncAuthorizeFilter));

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
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
            })
            .AddApplicationPart(mall)
            .AddFluentValidation(fv =>
            {
                //禁用其它的认证
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDistributedMemoryCache();

            #region Token

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Jwt";
                    options.DefaultChallengeScheme = "Jwt";
                }).AddJwtBearer("Jwt", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        //ValidAudience = "the audience you want to validate",
                        ValidateIssuer = false,
                        //ValidIssuer = "the isser you want to validate",

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long for HmacSha256")),

                        ValidateLifetime = true, //validate the expiration and not before values in the token

                        ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                    };
                });

            #endregion Token

            services.AddFluentValidation();

            #region Autofac

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<DefaultModule>();
            builder.Populate(services);
            IContainer container = builder.Build();
            return new AutofacServiceProvider(container);

            #endregion Autofac
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("any");

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
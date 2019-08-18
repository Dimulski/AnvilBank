using System;
using AnvilBank.Common.AutoMapping.Profiles;
using AnvilBank.Common.Configuration;
using AnvilBank.Common.EmailSender;
using AnvilBank.Common.Utils;
using AnvilBank.Data;
using AnvilBank.Models;
using AnvilBank.Web.Infrastructure.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace AnvilBank.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddDbContextPool<AnvilBankDbContext>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<BankUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<AnvilBankDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
            });

            services
                .AddDomainServices()
                .AddApplicationServices()
                .AddCommonProjectServices()
                .AddAuthentication();

            //services.Configure<SecurityStampValidatorOptions>(options => { options.ValidationInterval = TimeSpan.Zero; });

            services
                .Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services
                .Configure<BankConfiguration>(
                    this.Configuration.GetSection(nameof(BankConfiguration)))
                .Configure<SendGridConfiguration>(
                    this.Configuration.GetSection(nameof(SendGridConfiguration)));

            services
                .PostConfigure<BankConfiguration>(settings =>
                {
                    if (!ValidationUtil.IsObjectValid(settings))
                    {
                        throw new ApplicationException("BankConfiguration is invalid");
                    }
                })
                .PostConfigure<SendGridConfiguration>(settings =>
                {
                    if (!ValidationUtil.IsObjectValid(settings))
                    {
                        throw new ApplicationException("SendGridConfiguration is invalid");
                    }
                });

            services.Configure<SendGridConfiguration>(
                this.Configuration.GetSection(nameof(SendGridConfiguration)));
            services.PostConfigure<SendGridConfiguration>(settings =>
            {
                if (!ValidationUtil.IsObjectValid(settings))
                {
                    throw new ApplicationException("SendGridConfiguration is invalid.");
                }
            });

            services
                .AddResponseCompression(options => options.EnableForHttps = true);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Mapper.Initialize(config => config.AddProfile<DefaultProfile>());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.AddDefaultSecurityHeaders(
            //    new SecurityHeadersBuilder()
            //        .AddDefaultSecurePolicy());

            app.UseResponseCompression();
            //app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseStaticFiles(
                new StaticFileOptions
                {
                    OnPrepareResponse = ctx =>
                    {
                        const int cacheDurationInSeconds = 60 * 60 * 24 * 365; // 1 year
                        ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                            $"public,max-age={cacheDurationInSeconds}";
                    }
                });
            //app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseAuthentication();
            //app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            app.InitializeDatabase();
        }
    }
}

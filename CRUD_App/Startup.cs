using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CRUD_App.API.Middlewares;
using CRUD_App.General.Response;
using CRUD_App.Web.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace CRUD_App
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
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<IFileProvider>(
           new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // For Setting Session Timeout
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".EventCore";
            });

            services.AddSession();
            services.AddRouting();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped(typeof(IDataTableResponseHandler<>), typeof(DataTableResponseHandler<>));
            services.AddScoped(typeof(IObjectResponseHandler<>), typeof(ObjectResponseHandler<>));

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.WithOrigins(Configuration.GetSection("Apiconfig").GetSection("Weburl").Value.Trim('/') + "/"));
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie();

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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseCors(options => options.WithOrigins(Configuration.GetSection("Apiconfig").GetSection("baseurl").Value.Trim('/') + "/"));
            app.UseCors(options => options.WithOrigins(Configuration.GetSection("Apiconfig").GetSection("Weburl").Value.Trim('/') + "/"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();
            app.UseHttpsRedirection();

            app.UseMiddleware<AuthMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "areas",
                template: "{area=Admin}/{controller=Account}/{action=Login}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{area:exists}/{controller=Account}/{action=Login}/{id?}"
            //    );
            //});

            URLHelper.WebRootPath = Configuration.GetSection("Apiconfig").GetSection("Weburl").Value.Trim('/') + "/";
        }
    }
}

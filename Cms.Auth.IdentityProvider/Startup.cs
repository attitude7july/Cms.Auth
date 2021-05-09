using Cms.Auth.IdentityProvider.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Cms.Auth.IdentityProvider
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            

            services.AddControllersWithViews();
            //services.AddRazorPages();

            services.AddIdentityServer()
            .AddSigningCredential(InMemoryConfiguration.GetX509Certificate2())
            .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources)
            .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources)
            .AddInMemoryClients(InMemoryConfiguration.GetApiClients)
            .AddTestUsers(InMemoryConfiguration.GetApiUsers);
           
            
            services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
            {
                b.WithOrigins(Environment.GetEnvironmentVariable("CLIENT_URL"))
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
            services.AddMvcCore(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseIdentityServer();
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}

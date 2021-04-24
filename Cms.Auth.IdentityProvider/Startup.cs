using Cms.Auth.IdentityProvider.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //services.AddRazorPages();
            //configure identity server to use as service
            services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddInMemoryPersistedGrants()
            .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources)
            .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources)
            .AddInMemoryClients(InMemoryConfiguration.GetApiClients)
            .AddTestUsers(InMemoryConfiguration.GetApiUsers);
            //.AddInMemoryApiScopes(InMemoryConfiguration.GetApiScopes)
            ////.AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources)
            //.AddDeveloperSigningCredential()
            ////.AddSigningCredential(new X509Certificate2(Environment.GetEnvironmentVariable("CERTIFICATE_PATH"), Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD")))
            //.AddInMemoryClients(InMemoryConfiguration.GetApiClients)
            //.AddInMemoryApiResources(InMemoryConfiguration.GetApiResources);
            //Collection of different apis allow to use our authorization service
            services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
            {
                b.WithOrigins(Environment.GetEnvironmentVariable("CLIENT_REDIRECT_URL"))
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            //use identity server
            app.UseIdentityServer();

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

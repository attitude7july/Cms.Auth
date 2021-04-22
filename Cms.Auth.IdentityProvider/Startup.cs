using Cms.Auth.IdentityProvider.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

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
            services.AddMvc();
            //configure identity server to use as service
            services.AddIdentityServer()
                 .AddInMemoryApiScopes(InMemoryConfiguration.GetApiScopes)
                 .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources)
                 .AddDeveloperSigningCredential()
                //.AddSigningCredential(new X509Certificate2(Environment.GetEnvironmentVariable("CERTIFICATE_PATH"), Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD")))
                .AddTestUsers(InMemoryConfiguration.GetApiUsers)
                .AddInMemoryClients(InMemoryConfiguration.GetApiClients)
                .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources);
                //Collection of different apis allow to use our authorization service
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
            //use identity server
            app.UseIdentityServer();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
